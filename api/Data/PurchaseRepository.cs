using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class PurchaseRepository : IPurchaseRepository
    {
        private readonly ApplicationDbContext _context;

        public PurchaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Purchase> AddAsync(Purchase purchase)
        {
            await _context.AddAsync(purchase);
            await _context.SaveChangesAsync();

            return purchase;
        }

        public async Task<Purchase?> GetByIdAsync(int id)
        {
            var purchase = await _context.Purchases
                .SingleOrDefaultAsync(e => e.Id == id);

            return purchase;
        }

        public async Task<List<Purchase>?> GetByUserIdAsync(int userId)
        {
            var purchases = await _context.Purchases
                .Where(e => e.UserId == userId)
                .ToListAsync();

            return purchases;
        }

        public async Task<List<Purchase>?> GetAllAsync()
        {
            var purchases = await _context.Purchases
                .ToListAsync();

            return purchases;
        }

        public async Task<Purchase> UpdateAsync(Purchase purchase)
        {
            _context.Update(purchase);
            await _context.SaveChangesAsync();

            return purchase;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Purchase purchase)
        {
            _context.Remove(purchase);
            await _context.SaveChangesAsync();
        }
    }
}
