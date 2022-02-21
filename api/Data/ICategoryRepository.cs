using api.Models;

namespace api.Data
{
    public interface ICategoryRepository 
    {
        Task<Category> AddAsync(Category category);
        Task<List<Category>> FindCategoryWithItemsAsync(string searchKey);
        Task<Category?> GetByIdAsync(int id);
        Task<Category?> GetByIdWithItemsAsync(int id);
        Task<Category?> GetByNameAsync(string name);
        Task<Category?> GetByNameWithItemsAsync(string name);
        Task<List<Category>?> GetAllAsync();
        Task<List<Category>?> GetAllWithItemsAsync();
        Task<Category?> UpdateAsync(Category category);
        Task SaveChangesAsync();
        Task DeleteAsync(Category category);
    }
}
