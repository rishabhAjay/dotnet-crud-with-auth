using HotelListing.API.Core.Contracts;
using HotelListing.API.Core.Exceptions;
using HotelListing.API.Core.Models.Users;
using Microsoft.AspNetCore.Mvc;

namespace hotelListingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersRepository _usersRepository;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUsersRepository usersRepository, ILogger<UsersController> logger)
        {
            _usersRepository = usersRepository;
            _logger = logger;
        }


        //post: api/users/register
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        //enforce that the data is really coming from the body
        public async Task<ActionResult> Register([FromBody] ApiUserDto apiUserDto)
        {
            _logger.LogInformation($"Registration Attempt for {apiUserDto.Email}");
            //try
            //{
            var errors = await _usersRepository.registerUser(apiUserDto);

            //if there are any errors
            if (errors.Any())
            {
                foreach (var error in errors)
                {
                    //AddModelError accepts two params
                    ModelState.AddModelError(error.Code, error.Description);
                }

                throw new BadRequestException(nameof(Register), ModelState);
            }
            return Ok();
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex, $"Something went wrong in {nameof(Register)}");
            //    return Problem($"Something went wrong in {nameof(Register)}. Please contact support"
            //        , statusCode: 500);
            //}

        }

        //post: api/users/login
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Login([FromBody] LoginUserDto loginUserDto)
        {
            _logger.LogInformation($"Login Attempt for {loginUserDto.Email}");
            //try
            //{
            var loginResponse = await _usersRepository.loginUser(loginUserDto);

            //if there are any errors
            if (loginResponse == null)
            {
                throw new UnauthorizedException(nameof(Login), loginUserDto.Email);
            }
            else
                return Ok(loginResponse);

            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex, $"Something went wrong in {nameof(Login)}");
            //    return Problem($"Something went wrong in {nameof(Login)}. Please contact support"
            //        , statusCode: 500);
            //}


        }
        //POST: /api/users/refreshtoken
        [HttpPost]
        [Route("refreshtoken")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> RefreshToken([FromBody] LoginResponseDto loginUserDto)
        {
            var loginResponse = await _usersRepository.verifyRefreshToken(loginUserDto);

            //if there are any errors
            if (loginResponse == null)
            {
                throw new UnauthorizedException(nameof(RefreshToken), loginUserDto.RefreshToken);
            }
            else
                return Ok(loginResponse);


        }
    }
}
