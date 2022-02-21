using api.Models;

namespace api.Data
{
    public interface IUserRepository
    {
        Task<User> AddAsync(User user);
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByEmailAsync(string email);
        Task<List<User>> GetAllAsync();
        Task<List<User>> FindUserAsync(string searchKey);
        Task UpdateAsync(User user);
        Task SaveChangesAsync();
        Task DeleteAsync(User user);
    }
}
