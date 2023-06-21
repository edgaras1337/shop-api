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

        [HttpPost("add/{itemId:int}")]
        [Authorize]
        public async Task<ActionResult<AddToCartResponse>> AddToCart(int itemId)
        {
            try
            {
                var response = await _cartService.AddToCartAsync(itemId);
                return Ok(response);
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

        [HttpDelete("remove/{itemId:int}")]
        [Authorize]
        public async Task<ActionResult<RemoveFromCartResponse>> RemoveFromCart(int itemId)
        {
            try
            {
                var response = await _cartService.RemoveFromCartAsync(itemId);
                return Ok(response);
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
        public async Task<ActionResult<GetCartResponse>> GetCurrentUserCart()
        {
            try
            {
                var response = await _cartService.GetCurrentUserCart();
                return Ok(response);
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
