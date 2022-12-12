using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;

        public CartRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Cart> AddAsync(Cart cart)
        {
            await _context.AddAsync(cart);
            await _context.SaveChangesAsync();

            return cart;
        }

        public async Task<Cart?> GetCartWithItemsByIdAsync(int id)
        {
            return await _context.Carts
                .Include(e => e.CartItems)
                    .ThenInclude(e => e.Item)
                        .ThenInclude(e => e!.Images)
                .Include(e => e.CartItems)
                    .ThenInclude(e => e.Item)
                        .ThenInclude(e => e!.Category)
                .SingleOrDefaultAsync(e => e.UserId == id);
        }

        public async Task<List<Cart>?> GetAllCartsWithItemsAsync()
        {
            return await _context.Carts
                .Include(e => e.CartItems)
                    .ThenInclude(e => e.Item)
                        .ThenInclude(e => e!.Images)
                .Include(e => e.CartItems)
                    .ThenInclude(e => e.Item)
                        .ThenInclude(e => e!.Category)
                .ToListAsync();
        }

        public async Task<List<Cart>?> GetCartsByItemId(int itemId)
        {
            return await _context.Carts
                .Include(e => e.CartItems)
                    .ThenInclude(e => e.Item)
                .Where(e => e.CartItems.FirstOrDefault(cartItem => cartItem.ItemId == itemId) != null)
                .ToListAsync();
        }

        public async Task<Cart?> UpdateAsync(Cart cart)
        {
            _context.Update(cart);
            await _context.SaveChangesAsync();

            return cart;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task ClearCartAsync(Cart cart)
        {
            _context.CartItems.RemoveRange(cart.CartItems);
            await _context.SaveChangesAsync();
        }
    }
}
