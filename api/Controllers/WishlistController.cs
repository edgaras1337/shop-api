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
            try
            {
                var wishlistItems = await _wishlistItemService.AddToWishlistAsync(itemId);

                return Ok(wishlistItems ?? new List<AddWishlistItemResponse>());
            }
            catch (UnauthorizedException)
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
        }

        [HttpDelete("remove/{itemId}")]
        [Authorize]
        public async Task<ActionResult<List<RemoveWishlistItemResponse>>> RemoveWishlistItem(int itemId)
        {
            try
            {
                var wishlistItems = await _wishlistItemService.RemoveWishlistItemAsync(itemId);

                return Ok(wishlistItems ?? new List<RemoveWishlistItemResponse>());
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

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<GetCurrentUserWishlistResponse>>> GetCurrentUserWishlist()
        {
            try
            {
                var wishlistItems = await _wishlistItemService.GetCurrentUserWishlistAsync();

                return Ok(wishlistItems);
            }
            catch (UnauthorizedException)
            {
                return Unauthorized();
            }
        }
    }
}
