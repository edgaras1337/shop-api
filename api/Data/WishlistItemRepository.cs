using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class WishlistItemRepository : IWishlistItemRepository
    {
        private readonly DataContext _context;

        public WishlistItemRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<WishlistItem> AddAsync(WishlistItem wishlistItem)
        {
            await _context.AddAsync(wishlistItem);
            await _context.SaveChangesAsync();

            return wishlistItem;
        }

        public async Task<WishlistItem?> GetWishlistItemByIdAsync(int id)
        {
            return await _context.WishlistItems
                 .Include(e => e.Item)
                    .ThenInclude(e => e!.Images)
                .Include(e => e.Item)
                    .ThenInclude(e => e!.Category)
                .SingleOrDefaultAsync(e => e.Id == id);
        }

        public async Task<WishlistItem?> GetCartItemByUserAndItemIdAsync(int userId, int itemId)
        {
            return await _context.WishlistItems
                .Include(e => e.Item)
                    .ThenInclude(e => e!.Images)
                .Include(e => e.Item)
                    .ThenInclude(e => e!.Category)
                .SingleOrDefaultAsync(e => e.ItemId == itemId && e.UserId == userId);
        }

        public async Task<List<WishlistItem>?> GetWishlistByUserId(int userId)
        {
            return await _context.WishlistItems
                 .Include(e => e.Item)
                    .ThenInclude(e => e!.Images)
                .Include(e => e.Item)
                    .ThenInclude(e => e!.Category)
                .Where(e => e.UserId == userId)
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(WishlistItem wishlistItem)
        {
            _context.Remove(wishlistItem);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(List<WishlistItem> wishlistItems)
        {
            _context.RemoveRange(wishlistItems);
            await _context.SaveChangesAsync();
        }
    }
}
