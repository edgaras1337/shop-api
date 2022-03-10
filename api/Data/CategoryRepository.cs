using api.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace api.Data
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Category> AddAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return category;
        }

        public async Task<List<Category>> FindCategoryWithItemsAsync(string searchKey)
        {
            return await _context.Categories
                .Where(e => e.Name.Contains(searchKey))
                .ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            var category = await _context.Categories
                .SingleOrDefaultAsync(e => e.Id == id);

            return category;

        }
        public async Task<Category?> GetByIdWithItemsAsync(int id)
        {
            return await _context.Categories
                .Include(e => e.Items)
                .SingleOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Category?> GetByNameAsync(string name)
        {
            return await _context.Categories
                .SingleOrDefaultAsync(e => e.Name == name);
        }
        public async Task<Category?> GetByNameWithItemsAsync(string name)
        {
            return await _context.Categories
                .Include(e => e.Items)
                .SingleOrDefaultAsync(e => e.Name == name);
        }

        public async Task<List<Category>?> GetAllAsync()
        {
            return await _context.Categories
                .ToListAsync();
        }
        public async Task<List<Category>?> GetAllWithItemsAsync()
        {
            return await _context.Categories
                .Include(e => e.Items)
                .ToListAsync();
        }

        public async Task<Category?> UpdateAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();

            return category;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Category category)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}
