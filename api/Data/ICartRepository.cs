using api.Models;

namespace api.Data
{
    public interface ICartRepository
    {
        Task<Cart> AddAsync(Cart cart);
        Task<Cart?> GetCartByIdAsync(int id);
        Task<Cart?> GetCartWithItemsByIdAsync(int id);
        Task<List<Cart>?> GetAllCartsWithItemsAsync();
        Task<List<Cart>?> GetCartsByItemId(int itemId);
        //Task<CartItem?> GetCartItemByItemAndCartId(int itemId, int cartId);
        Task<Cart?> UpdateAsync(Cart cart);
        Task SaveChangesAsync();
        Task ClearCartAsync(Cart cart);
    }
}
