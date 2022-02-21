using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class ItemRepository : IItemRepository
    {
        private readonly DataContext _context;

        public ItemRepository(DataContext context)
        {
            _context = context;
        }


        public async Task<Item> AddAsync(Item item)
        {
            await _context.Items.AddAsync(item);
            await _context.SaveChangesAsync();

            return item;
        }

        public async Task<Item?> GetByIdAsync(int id)
        {
            var item = await _context.Items
                .Include(e => e.Category)
                .Include(e => e.Images)
                .Include(e => e.Comments)
                    .ThenInclude(e => e.User)
                        .ThenInclude(e => e!.UserRoles)
                            .ThenInclude(e => e!.Role)
                .SingleOrDefaultAsync(e => e.Id == id);

            return item;
        }

        public async Task<List<Item>> GetByCategoryIdAsync(int categoryId)
        {
            var items = await _context.Items
                .Include(e => e.Category)
                .Include(e => e.Images)
                .Include(e => e.Comments)
                    .ThenInclude(e => e.User)
                        .ThenInclude(e => e!.UserRoles)
                            .ThenInclude(e => e!.Role)
                .Where(e => e.CategoryId == categoryId)
                .ToListAsync();

            return items;
        }

        public async Task<List<Item>> FindAsync(string searchKey)
        {
            var items = await _context.Items
                .Include(e => e.Category)
                .Include(e => e.Images)
                .Include(e => e.Comments)
                    .ThenInclude(e => e.User)
                        .ThenInclude(e => e!.UserRoles)
                            .ThenInclude(e => e!.Role)
                .Where(e => e.Name.Contains(searchKey))
                .ToListAsync();

            return items;
        }

        public async Task<List<Item>> GetAllAsync()
        {
            return await _context.Items
                .Include(e => e.Category)
                .Include(e => e.Images)
                .Include(e => e.Comments)
                    .ThenInclude(e => e.User)
                        .ThenInclude(e => e!.UserRoles)
                            .ThenInclude(e => e!.Role)
                .ToListAsync();
        }

        public async Task<Item> UpdateAsync(Item item)
        {
            _context.Update(item);
            await _context.SaveChangesAsync();

            return item;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Item item)
        {
            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}
