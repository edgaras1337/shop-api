using api.CustomExceptions;
using api.Dtos.WishlistControllerDtos;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistItemService _wishlistItemService;

        public WishlistController(IWishlistItemService wishlistItemService)
        {
            _wishlistItemService = wishlistItemService;
        }

        [HttpPost("add/{itemId}")]
        [Authorize]
        public async Task<ActionResult<List<AddWishlistItemResponse>>> AddToWishlist(int itemId)
        {
            List<AddWishlistItemResponse>? wishlistItems;
            try
            {
                wishlistItems = await _wishlistItemService.AddToWishlistAsync(itemId);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (ObjectNotFoundException)
            {
                return NotFound();
            }
            catch (DuplicateDataException)
            {
                return Conflict();
            }

            return Ok(wishlistItems ?? new List<AddWishlistItemResponse>());
        }

        [HttpDelete("remove/{itemId}")]
        [Authorize]
        public async Task<ActionResult<List<RemoveWishlistItemResponse>>> RemoveWishlistItem(int itemId)
        {
            List<RemoveWishlistItemResponse>? wishlistItems;
            try
            {
                wishlistItems = await _wishlistItemService.RemoveWishlistItemAsync(itemId);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (ObjectNotFoundException)
            {
                return NotFound();
            }

            return Ok(wishlistItems ?? new List<RemoveWishlistItemResponse>());
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<GetCurrentUserWishlistResponse>>> GetCurrentUserWishlist(int itemId)
        {
            List<GetCurrentUserWishlistResponse>? wishlistItems;
            try
            {
                wishlistItems = await _wishlistItemService.GetCurrentUserWishlistAsync();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }

            return Ok(wishlistItems);
        }
    }
}
