using api.CustomExceptions;
using api.Dtos.CommentControllerDtos;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/item/reviews")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService commentService)
        {
            _reviewService = commentService;
        }

        // adds a comment and returns item dto containing all items
        [HttpPost("add")]
        [Authorize]
        public async Task<ActionResult<AddReviewResponse>> AddReview(AddReviewRequest dto)
        {
            try
            {
                var item = await _reviewService.AddReviewAsync(dto);
                return Ok(item);
            }
            catch (UnauthorizedException)
            {
                return Unauthorized();
            }
            catch (ObjectNotFoundException)
            {
                return NotFound();
            }
        }

        // updates the comment and returns item dto containing all comments
        [HttpPut("update")]
        [Authorize]
        public async Task<ActionResult<UpdateReviewResponse>> UpdateReview(UpdateReviewRequest dto)
        {
            try
            {
                var item = await _reviewService.UpdateReviewAsync(dto);
                return Ok(item);
            }
            catch (UnauthorizedException)
            {
                return Unauthorized();
            }
            catch (ObjectNotFoundException)
            {
                return NotFound();
            }
        }

        // removes a comment and returns the item dto containing all items
        [HttpDelete("remove/{reviewId:int}")]
        [Authorize]
        public async Task<ActionResult> RemoveReview(int reviewId)
        {
            try
            {
                await _reviewService.RemoveCommentByIdAsync(reviewId);
                return NoContent();
            }
            catch (UnauthorizedException)
            {
                return Unauthorized();
            }
            catch (ObjectNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
