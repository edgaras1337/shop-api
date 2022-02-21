using api.CustomExceptions;
using api.Dtos.CommentControllerDtos;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/item/comments")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }


        // adds a comment and returns item dto containing all items
        [HttpPost("add")]
        [Authorize]
        public async Task<ActionResult<AddCommentResponse>> AddComment(AddCommentRequest dto)
        {
            AddCommentResponse? item;
            try
            {
                item = await _commentService.AddCommentAsync(dto);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (ObjectNotFoundException)
            {
                return NotFound();
            }

            return Ok(item);
        }

        // updates the comment and returns item dto containing all comments
        [HttpPut("update")]
        [Authorize]
        public async Task<ActionResult<UpdateCommentResponse>> UpdateComment(UpdateCommentRequest dto)
        {
            UpdateCommentResponse? item;
            try
            {
                item = await _commentService.UpdateCommentAsync(dto);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (ObjectNotFoundException)
            {
                return NotFound();
            }

            return Ok(item);
        }

        // removes a comment and returns the item dto containing all items
        [HttpDelete("remove/{commentId}")]
        [Authorize]
        public async Task<ActionResult<RemoveCommentResponse>> RemoveComment(int commentId)
        {
            RemoveCommentResponse? item;
            try
            {
                item = await _commentService.RemoveCommentByIdAsync(commentId);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (ObjectNotFoundException)
            {
                return NotFound();
            }

            return Ok(item);
        }
    }
}
