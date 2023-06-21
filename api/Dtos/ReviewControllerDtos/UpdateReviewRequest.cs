using api.Models;
using AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace api.Dtos.CommentControllerDtos
{
    public class UpdateReviewRequest
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MinLength(1)]
        public string CommentText { get; set; } = string.Empty;
    }

    public class UpdateCommentRequestProfiles : Profile
    {
        public UpdateCommentRequestProfiles()
        {
            CreateMap<UpdateReviewRequest, ItemReview>();
        }
    }
}
