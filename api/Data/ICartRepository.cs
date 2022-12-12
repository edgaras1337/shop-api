using api.Models;

namespace api.Data
{
    public interface ICartRepository
    {
        Task<Cart> AddAsync(Cart cart);
        Task<Cart?> GetCartWithItemsByIdAsync(int id);
        Task<List<Cart>?> GetAllCartsWithItemsAsync();
        Task<List<Cart>?> GetCartsByItemId(int itemId);
        Task<Cart?> UpdateAsync(Cart cart);
        Task SaveChangesAsync();
        Task ClearCartAsync(Cart cart);
    }
}
