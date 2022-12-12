using api.Models;

namespace api.Data
{
    public interface IPurchaseRepository
    {
        Task<Purchase> AddAsync(Purchase purchase);
        Task<Purchase?> GetByIdAsync(int id);
        Task<List<Purchase>?> GetByUserIdAsync(int userId);
        Task<List<Purchase>?> GetAllAsync();
        Task<Purchase> UpdateAsync(Purchase purchase);
        Task SaveChangesAsync();
        Task DeleteAsync(Purchase purchase);
    }
}
