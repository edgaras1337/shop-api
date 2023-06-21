using api.Dtos.CartControllerDtos;
using api.Models;
using System.Linq.Expressions;

namespace api.Services
{
    public interface ICartService
    {
        /*
         * add to cart              X
         * remove from cart         X
         * get cart items           
         * get single cart item
         * clear all cart items
         */

        Task<AddToCartResponse> AddToCartAsync(int itemId);
        Task<RemoveFromCartResponse?> RemoveFromCartAsync(int itemId);
        Task<GetCartResponse> GetCurrentUserCart();
        Task RecalcCartPrice(Expression<Func<Cart, bool>> cartFilter);
    }
}
