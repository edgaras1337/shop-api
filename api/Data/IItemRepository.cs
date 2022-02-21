using api.Models;

namespace api.Data
{
    public interface IItemRepository
    {
        Task<Item> AddAsync(Item item);
        Task<Item?> GetByIdAsync(int id);
        Task<List<Item>> GetByCategoryIdAsync(int categoryId);
        Task<List<Item>> FindAsync(string searchKey);
        Task<List<Item>> GetAllAsync();
        Task<Item> UpdateAsync(Item item);
        Task SaveChangesAsync();
        Task DeleteAsync(Item item);
    }
}
