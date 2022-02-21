using api.Models;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace api.Data
{
    public class RoleRepository : IRoleRepository
    {
        private readonly DataContext _context;

        public RoleRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Role> AddAsync(Role role)
        {
            await _context.AddAsync(role);
            await _context.SaveChangesAsync();

            return role;
        }

        public async Task<Role> GetByIdAsync(int id)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(e => e.Id == id);
        }
        public async Task<Role> GetByRoleNameAsync(string roleName)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(e => e.RoleName == roleName);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Role role)
        {
            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
        }
    }
}
