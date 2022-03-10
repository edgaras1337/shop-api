using api.Models;

namespace api.Data
{
    public interface ICartItemRepository
    {
        Task<CartItem> AddAsync(CartItem cartItem);
        Task<CartItem?> GetCartItemByIdAsync(int id);
        Task<CartItem?> GetCartItemByCartAndItemIdAsync(string cartId, int itemId);
        Task<List<CartItem>?> GetCartItemsByCartIdAsync(string cartId);
        Task SaveChangesAsync();
        Task DeleteAsync(CartItem cartItem);
        Task DeleteRangeAsync(List<CartItem> cartItems);
    }
}
