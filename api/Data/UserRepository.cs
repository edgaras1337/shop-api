using api.Helpers;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        // CREATE
        public async Task<User> AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        // READ
        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users
                .Include(e => e.UserRoles)
                    .ThenInclude(e => e.Role)
                .Include(e => e.Cart)
                    .ThenInclude(e => e!.CartItems)
                        .ThenInclude(e => e.Item)
                            .ThenInclude(e => e!.Images)
                .Include(e => e.Cart!.CartItems)
                    .ThenInclude(e => e.Item!.Category)
                .Include(e => e.WishlistItems)
                    .ThenInclude(e => e.Item)
                        .ThenInclude(e => e!.Images)
                .Include(e => e.WishlistItems)
                    .ThenInclude(e => e.Item!.Category)
                .SingleOrDefaultAsync(e => e.Id == id);
        }
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .Include(e => e.UserRoles)
                    .ThenInclude(e => e.Role)
                .Include(e => e.Cart)
                    .ThenInclude(e => e!.CartItems)
                        .ThenInclude(e => e.Item)
                            .ThenInclude(e => e!.Images)
                .Include(e => e.Cart!.CartItems)
                    .ThenInclude(e => e.Item!.Category)
                .Include(e => e.WishlistItems)
                    .ThenInclude(e => e.Item)
                        .ThenInclude(e => e!.Images)
                .Include(e => e.WishlistItems)
                    .ThenInclude(e => e.Item!.Category)
                .SingleOrDefaultAsync(e => e.Email == email);
        }
        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users
                .Include(e => e.UserRoles)
                    .ThenInclude(e => e.Role)
                .ToListAsync();
        }

        public async Task<List<User>> FindUserAsync(string searchKey)
        {
            return await _context.Users
                .Include(e => e.UserRoles)
                    .ThenInclude(e => e.Role)
                .Where(e => 
                    e.Email.Contains(searchKey) ||
                    e.Name.Contains(searchKey) || 
                    e.Surname.Contains(searchKey) || 
                    e.Id.ToString().Contains(searchKey))
                .ToListAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(User user)
        {
            _context.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}
