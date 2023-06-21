using api.CustomExceptions;
using api.Dtos.AuthControllerDtos;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        public AuthController(IUserService userService)
        {
            _userService  = userService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<RegisterResponse>> RegisterAsync(RegisterRequest dto)
        {
            try
            {
                var response = await _userService.CreateUserAsync(dto);
                return CreatedAtAction(nameof(RegisterAsync), response);
            }
            catch (DuplicateDataException)
            {
                // Email is already in use.
                return Conflict();
            }
            catch (InvalidPasswordException)
            {
                return BadRequest();
            }
            catch (UserRegistrationException)
            {
                return BadRequest();
            }
            catch (ObjectNotFoundException)
            {
                // Role wasn't found.
                return NotFound();
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> LoginAsync(LoginRequest dto)
        {
            var user = await _userService.AuthenticateAsync(dto);
            if(user == null)
            {
                return Unauthorized();
            }

            return Ok(user);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<ActionResult> LogoutAsync()
        {
            await _userService.LogoutAsync();
            return NoContent();
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<GetCurrentUserResponse>> GetCurrentUserAsync()
        { 
            var user = await _userService.GetCurrentUserAsync();
            if (user == null)
            {
                return Unauthorized();
            }

            return Ok(user);
        }
    }
}
