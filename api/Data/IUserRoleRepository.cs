using api.Models;

namespace api.Data
{
    public interface IUserRoleRepository
    {
        Task<UserRole> AddAsync(UserRole userRole);
        Task<List<UserRole>> AddRangeAsync(List<UserRole> userRoles);
        Task DeleteAsync(UserRole userRole);
    }
}
