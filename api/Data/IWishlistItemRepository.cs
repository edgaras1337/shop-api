using api.Models;

namespace api.Data
{
    public interface IWishlistItemRepository
    {
        Task<WishlistItem> AddAsync(WishlistItem wishlistItem);
        Task<WishlistItem?> GetWishlistItemByIdAsync(int id);
        Task<WishlistItem?> GetCartItemByUserAndItemIdAsync(string userId, int itemId);
        Task<List<WishlistItem>?> GetWishlistByUserId(string userId);
        Task SaveChangesAsync();
        Task DeleteAsync(WishlistItem wishlistItem);
        Task DeleteRangeAsync(List<WishlistItem> wishlistItems);
    }
}
