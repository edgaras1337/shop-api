using api.Models;

namespace api.Data
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly DataContext _context;

        public UserRoleRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<UserRole> AddAsync(UserRole userRole)
        {
            await _context.UserRoles.AddAsync(userRole);
            await _context.SaveChangesAsync();

            return userRole;
        }

        public async Task<List<UserRole>> AddRangeAsync(List<UserRole> userRoles)
        {
            await _context.UserRoles.AddRangeAsync(userRoles);
            await _context.SaveChangesAsync();

            return userRoles;
        }

        public async Task DeleteAsync(UserRole userRole)
        {
            _context.UserRoles.Remove(userRole);
            await _context.SaveChangesAsync();
        }
    }
}
