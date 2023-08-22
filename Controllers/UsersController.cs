using hotelListingAPI.Contracts;
using hotelListingAPI.Models.Users;
using Microsoft.AspNetCore.Mvc;

namespace hotelListingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersRepository _usersRepository;

        public UsersController(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
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
            var errors = await _usersRepository.registerUser(apiUserDto);

            //if there are any errors
            if (errors.Any())
            {
                foreach (var error in errors)
                {
                    //AddModelError accepts two params
                    ModelState.AddModelError(error.Code, error.Description);
                }

                return BadRequest(ModelState);
            }
            return Ok();
        }

        //post: api/users/login
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Login([FromBody] LoginUserDto loginUserDto)
        {
            var loginResponse = await _usersRepository.loginUser(loginUserDto);

            //if there are any errors
            if (loginResponse == null)
            {
                return Unauthorized();
            }
            else
                return Ok(loginResponse);


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
                return Unauthorized();
            }
            else
                return Ok(loginResponse);


        }
    }
}
