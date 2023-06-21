using api.Models;
using AutoMapper;

namespace api.Dtos.CommentControllerDtos
{
    public class AddReviewResponse
    {
        public int Id { get; set; }
        public string Comment { get; set; } = string.Empty;
        public int RatingCount { get; set; }
        public int ItemId { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }

        public UserDto? User { get; set; }

        public class UserDto
        {
            public int Id { get; set; }
            public string Email { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
            public string Surname { get; set; } = string.Empty;
            public List<UserRoleDto> UserRoles { get; set; } = new();

            public class UserRoleDto
            {
                public RoleDto? Role { get; set; }

                public class RoleDto
                {
                    public int Id { get; set; }
                    public string Name { get; set; } = string.Empty;
                }
            }
        }
    }

    // profiles for automapper
    public class AddReviewResponseProfiles : Profile
    {
        public AddReviewResponseProfiles()
        {
            CreateMap<ItemReview, AddReviewResponse>();
            CreateMap<ApplicationUser, AddReviewResponse.UserDto>();
            CreateMap<ApplicationUserRole, AddReviewResponse.UserDto.UserRoleDto>();
            CreateMap<ApplicationRole, AddReviewResponse.UserDto.UserRoleDto.RoleDto>();
        }
    }
}
