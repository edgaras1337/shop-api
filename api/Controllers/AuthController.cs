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
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService  = userService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<RegisterResponse>> Register(RegisterRequest dto)
        {
            try
            {
                // create user account and return response dto
                var response = await _userService.CreateUserAsync(dto);

                return CreatedAtAction(nameof(Register), response);
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
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest dto)
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
        public async Task<ActionResult> Logout()
        {
            await _userService.LogoutAsync();

            return NoContent();
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<GetCurrentUserResponse>> GetCurrentUser()
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
