using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _context;

        public CommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Comment> AddAsync(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();

            return comment;
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _context.Comments
                .SingleOrDefaultAsync(e => e.Id == id);
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            return await _context.Comments
                .Include(e => e.Item)
                .ToListAsync();
        }

        public async Task<List<Comment>> GetCommentsByItemIdAsync(int itemId)
        {
            return await _context.Comments
                .Include(e => e.Item)
                .Where(e => e.ItemId == itemId)
                .ToListAsync();
        }

        public async Task<List<Comment>> GetCommentsByUserIdAsync(string userId)
        {
            return await _context.Comments
                .Include(e => e.Item)
                .Where(e => e.UserId == userId)
                .ToListAsync();
        }
        public async Task<Comment> UpdateAsync(Comment comment)
        {
            _context.Update(comment);
            await _context.SaveChangesAsync();

            return comment;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Comment comment)
        {
            _context.Remove(comment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(List<Comment> comments)
        {
            _context.RemoveRange(comments);
            await _context.SaveChangesAsync();
        }
    }
}
