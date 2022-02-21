using api.Models;

namespace api.Data
{
    public interface IRoleRepository
    {
        Task<Role> AddAsync(Role role);
        Task<Role> GetByIdAsync(int id);
        Task<Role> GetByRoleNameAsync(string roleName);
        Task SaveChangesAsync();
        Task DeleteAsync(Role role);
    }
}
