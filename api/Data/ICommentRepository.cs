using api.Models;

namespace api.Data
{
    public interface ICommentRepository
    {
        Task<Comment> AddAsync(Comment comment);
        Task<Comment?> GetByIdAsync(int id);
        Task<List<Comment>> GetAllAsync();
        Task<List<Comment>> GetCommentsByItemIdAsync(int itemId);
        Task<List<Comment>> GetCommentsByUserIdAsync(string userId);
        Task<Comment> UpdateAsync(Comment comment);
        Task SaveChangesAsync();
        Task DeleteAsync(Comment comment);
        Task DeleteRangeAsync(List<Comment> comments);
    }
}
