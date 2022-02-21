﻿using api.Dtos.CartControllerDtos;

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
        Task<GetCartWithItemsResponse> GetCurrentUserCartWithItemsAsync();
    }
}