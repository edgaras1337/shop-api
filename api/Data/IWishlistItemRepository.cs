using api.Models;

namespace api.Data
{
    public interface IWishlistItemRepository
    {
        Task<WishlistItem> AddAsync(WishlistItem wishlistItem);
        Task<WishlistItem?> GetWishlistItemByIdAsync(int id);
        Task<WishlistItem?> GetCartItemByUserAndItemIdAsync(int userId, int itemId);
        Task<List<WishlistItem>?> GetWishlistByUserId(int userId);
        Task SaveChangesAsync();
        Task DeleteAsync(WishlistItem wishlistItem);
        Task DeleteRangeAsync(List<WishlistItem> wishlistItems);
    }
}
