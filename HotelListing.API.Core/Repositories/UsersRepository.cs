using AutoMapper;
using HotelListing.API.Core.Contracts;
using HotelListing.API.Core.Models.Users;
using HotelListing.API.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HotelListing.API.Core.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

        public UsersRepository(IMapper mapper, UserManager<User> userManager, IConfiguration configuration)
        {
            _mapper = mapper;
            _userManager = userManager;
            _configuration = configuration;
        }


        public async Task<IEnumerable<IdentityError>> registerUser(ApiUserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            user.UserName = userDto.Email;
            var result = await _userManager.CreateAsync(user, userDto.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
            }

            //we defined the return type to be IdentityError which means if this is successful
            //it returns empty
            return result.Errors;
        }

        public async Task<LoginResponseDto> loginUser(LoginUserDto userDto)
        {

            var user = await _userManager.FindByEmailAsync(userDto.Email);
            bool validatePassword = await _userManager.CheckPasswordAsync(user, userDto.Password);
            if (user == null || validatePassword == false)
            {
                return null;

            }

            var token = await generateToken(user);

            //return a new generated refreshtoken and send it to the user on login
            return new LoginResponseDto
            {
                Token = token,
                UserId = user.Id,
                RefreshToken = await createRefreshToken(user)
            };

        }


        public async Task<string> generateToken(User user)
        {
            //get the security key
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));

            //creating the signing with the key and algorithm
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //define role as a claim
            var roles = await _userManager.GetRolesAsync(user);

            //colate the roles coming from the database and map them to claimType constants
            var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList(); //"Role"

            //get claims for the user which you have to add while creating a user in the DB
            var userClaims = await _userManager.GetClaimsAsync(user);

            //these claims need to be present.
            var claims = new List<Claim>{
                //this is the subject to whom the token has been issued to
                new Claim(JwtRegisteredClaimNames.Sub, user.Email), //"Sub"
                //the Jti value changes on every token
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), //"Jti"
            //add Email or other user details as claims
                 new Claim(JwtRegisteredClaimNames.Email, user.Email), //"Email"
            //create your own static class and hardcode these constants there
                 new Claim("UID", user.Id)
        //append the other claims as well
            }.Union(userClaims).Union(roleClaims);

            //initialize the token claims and credentials
            var token = new JwtSecurityToken(issuer: _configuration["JwtSettings:Issuer"], audience: _configuration["JwtSettings:Audience"], claims: claims, expires: DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["JwtSettings:DurationInMinutes"])), signingCredentials: credentials);

            //return token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> createRefreshToken(User user)
        {
            //remove prevous refresh token
            await _userManager.RemoveAuthenticationTokenAsync(user, _configuration["JwtSettings:Issuer"], "RefreshToken");
            //create a refresh token
            var newRefreshToken = await _userManager.GenerateUserTokenAsync(user, _configuration["JwtSettings:Issuer"], "RefreshToken");
            //save that to db
            var result = await _userManager.SetAuthenticationTokenAsync(user, _configuration["JwtSettings:Issuer"], "RefreshToken", newRefreshToken);
            return newRefreshToken;
        }

        public async Task<LoginResponseDto> verifyRefreshToken(LoginResponseDto userDto)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            //read and decode the token
            var tokenContent = jwtSecurityTokenHandler.ReadJwtToken(userDto.Token);
            //get the username/email from the claim
            var email = tokenContent.Claims.ToList().FirstOrDefault(q => q.Type == JwtRegisteredClaimNames.Email)?.Value;
            Console.WriteLine("TOken CONTENT:: " + tokenContent.Claims.ToList().FirstOrDefault(q => q.Type == JwtRegisteredClaimNames.Email));
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return null;
            }
            //verify that the refreshToken that is coming from the request and the actual one stored is valid or not
            var isvalidRefreshToken = await _userManager.VerifyUserTokenAsync(user, _configuration["JwtSettings:Issuer"], "RefreshToken", userDto.RefreshToken);
            //if it is indeed valid then generate a new refreshtoken and store it in the DB. Also generate a new JWT and return
            if (isvalidRefreshToken)
            {
                return new LoginResponseDto
                {
                    RefreshToken = await createRefreshToken(user),
                    Token = await generateToken(user),
                    UserId = user.Id,
                };
            }
            //this regenerates a security stamp and signs you out
            await _userManager.UpdateSecurityStampAsync(user);
            return null;
        }
    }
}
