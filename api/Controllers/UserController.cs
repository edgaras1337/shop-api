using api.CustomExceptions;
using api.Dtos;
using api.Dtos.UserControllerDtos;
using api.Models;
using api.Services;
using api.UserControllerDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace api.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("update")]
        [Authorize]
        public async Task<ActionResult<UpdateUserResponse>> UpdateUser([FromForm] UpdateUserRequest request)
        {
            UpdateUserResponse response;
            try
            {
                response = await _userService.UpdateCurrentUserAsync(request);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (DuplicateNameException)
            {
                return Conflict();
            }
            catch (InvalidPasswordException)
            {
                return BadRequest();
            }

            return Ok(response);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<List<GetAllUsersResponse>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();

            return Ok(users);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<GetUserByIdResponse>> GetUserById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if(user is null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpGet("find")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<FindUserResponse>> FindUser(FindUserRequest request)
        {
            var users = await _userService.FindUserAsync(request);

            if(users is null)
            {
                return NotFound();
            }

            return Ok(users);
        }

        [HttpDelete("deactivate")]
        [Authorize]
        public async Task<ActionResult> Delete()
        {
            try
            {
                await _userService.DeactivateAccountAsync();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }

            return NoContent();
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            if(!(await _userService.DeleteUserByIdAsync(id)))
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}

