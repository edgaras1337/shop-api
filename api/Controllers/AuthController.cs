using api.CustomExceptions;
using api.Data;
using api.Dtos;
using api.Dtos.AuthControllerDtos;
using api.Helpers;
using api.Models;
using api.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

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
                // create user account and return response dto
                var response = await _userService.CreateUserAsync(dto);

                return CreatedAtAction(nameof(RegisterAsync), response);
            }
            catch (DuplicateDataException)
            {
                // email was already in use
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
                // role wasnt found
                return NotFound();
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> LoginAsync(LoginRequest dto)
        {
            // authenticate user and return response dto
            var user = await _userService.AuthenticateAsync(dto);
            if(user is null)
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
