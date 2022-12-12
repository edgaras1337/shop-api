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
    [Route("api/users")]
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
            try
            {
                var response = await _userService.UpdateCurrentUserAsync(request);

                return Ok(response);
            }
            catch (ObjectNotFoundException)
            {
                return NotFound();
            }
            catch (UnauthorizedException)
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

            if (user is null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpGet("find/{searchKey}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<FindUserResponse>> FindUser(string searchKey)
        {
            var users = await _userService.FindUserAsync(searchKey);

            if (users is null || users.Count == 0)
            {
                return NotFound();
            }

            return Ok(users);
        }

        [HttpDelete("deactivate")]
        [Authorize]
        public async Task<ActionResult> Delete()
        {
            if (!await _userService.DeactivateAccountAsync())
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> DeleteUser(string id)
        {
            if (!await _userService.DeleteUserByIdAsync(id))
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}

