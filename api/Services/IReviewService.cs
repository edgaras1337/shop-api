using api.Dtos.CommentControllerDtos;

namespace api.Services
{
    public interface IReviewService
    {
        Task<AddReviewResponse> AddReviewAsync(AddReviewRequest dto);
        Task<UpdateReviewResponse> UpdateReviewAsync(UpdateReviewRequest dto);
        Task RemoveCommentByIdAsync(int commentId);
    }
}
