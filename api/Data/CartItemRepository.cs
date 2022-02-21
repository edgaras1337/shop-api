using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class CartItemRepository : ICartItemRepository
    {
        private readonly DataContext _context;

        public CartItemRepository(DataContext context)
        {
            _context = context;
        }


        public async Task<CartItem> AddAsync(CartItem cartItem)
        {
            await _context.CartItems.AddAsync(cartItem);
            await _context.SaveChangesAsync();

            return cartItem;
        }

        public async Task<CartItem?> GetCartItemByIdAsync(int id)
        {
            return await _context.CartItems
                .Include(e => e.Item)
                    .ThenInclude(e => e!.Images)
                .Include(e => e.Item)
                    .ThenInclude(e => e!.Category)
                .SingleOrDefaultAsync(e => e.Id == id);
        }

        public async Task<CartItem?> GetCartItemByCartAndItemIdAsync(int cartId, int itemId)
        {
            return await _context.CartItems
                .Include(e => e.Item)
                    .ThenInclude(e => e!.Images)
                .Include(e => e.Item)
                    .ThenInclude(e => e!.Category)
                .SingleOrDefaultAsync(e => e.CartId == cartId && e.ItemId == itemId);
        }

        public async Task<List<CartItem>?> GetCartItemsByCartIdAsync(int cartId)
        {
            return await _context.CartItems
                .Include(e => e.Item)
                    .ThenInclude(e => e!.Images)
                .Include(e => e.Item)
                    .ThenInclude(e => e!.Category)
                .Where(e => e.CartId == cartId)
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(CartItem cartItem)
        {
            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(List<CartItem> cartItems)
        {
            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
        }
    }
}
