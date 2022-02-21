using api.CustomExceptions;
using api.Dtos.CartControllerDtos;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("add/{itemId}")]
        [Authorize]
        public async Task<ActionResult<AddToCartResponse>> AddToCart(int itemId)
        {
            AddToCartResponse? response;
            try
            {
                response = await _cartService.AddToCartAsync(itemId);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (ObjectNotFoundException)
            {
                return NotFound();
            }

            return Ok(response);
        }

        [HttpDelete("remove/{itemId}")]
        [Authorize]
        public async Task<ActionResult<RemoveFromCartResponse>> RemoveFromCart(int itemId)
        {
            RemoveFromCartResponse? response;
            try
            {
                response = await _cartService.RemoveFromCartAsync(itemId);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (ObjectNotFoundException)
            {
                return NotFound();
            }

            return Ok(response);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<GetCartWithItemsResponse>> GetCurrentUserCart()
        {
            GetCartWithItemsResponse response;
            try
            {
                response = await _cartService.GetCurrentUserCartWithItemsAsync();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (ObjectNotFoundException)
            {
                return NotFound();
            }

            return Ok(response);
        }
    }
}
