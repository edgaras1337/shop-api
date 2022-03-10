using api.Models;
using AutoMapper;

namespace api.Dtos.CommentControllerDtos
{
    public class UpdateCommentResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public UpdateCommentResponse_CategoryDto? Category { get; set; }
        public List<UpdateCommentResponse_ItemImagesDto>? Images { get; set; }
        public List<UpdateCommentResponse_CommentDto> Comments { get; set; } = new();
    }


    // navigation dtos
    public class UpdateCommentResponse_CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class UpdateCommentResponse_ItemImagesDto
    {
        public int Id { get; set; }
        public string ImageName { get; set; } = string.Empty;
        public string ImageSource { get; set; } = string.Empty;
    }

    public class UpdateCommentResponse_CommentDto
    {
        public int Id { get; set; }
        public string CommentText { get; set; } = string.Empty;
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public UpdateCommentResponse_UserDto? User { get; set; }
    }

    public class UpdateCommentResponse_UserDto
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public List<UpdateCommentResponse_UserRoleDto> UserRoles { get; set; } = new();
    }

    public class UpdateCommentResponse_UserRoleDto
    {
        public UpdateCommentResponse_RoleDto? Role { get; set; }
    }

    public class UpdateCommentResponse_RoleDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }


    // profiles for automapper
    public class UpdateCommentResponseProfiles : Profile
    {
        public UpdateCommentResponseProfiles()
        {
            CreateMap<Item, UpdateCommentResponse>();
            CreateMap<Category, UpdateCommentResponse_CategoryDto>();
            CreateMap<ItemImage, UpdateCommentResponse_ItemImagesDto>();
            CreateMap<Comment, UpdateCommentResponse_CommentDto>();
            CreateMap<ApplicationUser, UpdateCommentResponse_UserDto>();
            CreateMap<ApplicationUserRole, UpdateCommentResponse_UserRoleDto>();
            CreateMap<ApplicationRole, UpdateCommentResponse_RoleDto>();
        }
    }
}
