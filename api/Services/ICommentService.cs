using api.Dtos.CommentControllerDtos;

namespace api.Services
{
    public interface ICommentService
    {
        Task<AddCommentResponse> AddCommentAsync(AddCommentRequest dto);
        Task<UpdateCommentResponse> UpdateCommentAsync(UpdateCommentRequest dto);
        Task<RemoveCommentResponse> RemoveCommentByIdAsync(int commentId);
    }
}
