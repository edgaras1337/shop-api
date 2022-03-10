using api.Models;
using AutoMapper;

namespace api.Dtos.CommentControllerDtos
{
    public class RemoveCommentResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public RemoveComment_CategoryDto? Category { get; set; }
        public List<RemoveComment_ItemImagesDto>? Images { get; set; }
        public List<RemoveComment_CommentDto> Comments { get; set; } = new();
    }


    // navigation dtos
    public class RemoveComment_CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class RemoveComment_ItemImagesDto
    {
        public int Id { get; set; }
        public string ImageName { get; set; } = string.Empty;
        public string ImageSource { get; set; } = string.Empty;
    }

    public class RemoveComment_CommentDto
    {
        public int Id { get; set; }
        public string CommentText { get; set; } = string.Empty;
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public RemoveComment_UserDto? User { get; set; }
    }

    public class RemoveComment_UserDto
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public List<RemoveComment_UserRoleDto> UserRoles { get; set; } = new();
    }

    public class RemoveComment_UserRoleDto
    {
        public RemoveComment_RoleDto? Role { get; set; }
    }

    public class RemoveComment_RoleDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }


    // profiles for automapper
    public class RemoveCommentResponseProfiles : Profile
    {
        public RemoveCommentResponseProfiles()
        {
            CreateMap<Item, RemoveCommentResponse>();
            CreateMap<Category, RemoveComment_CategoryDto>();
            CreateMap<ItemImage, RemoveComment_ItemImagesDto>();
            CreateMap<Comment, RemoveComment_CommentDto>();
            CreateMap<ApplicationUser, RemoveComment_UserDto>();
            CreateMap<ApplicationUserRole, RemoveComment_UserRoleDto>();
            CreateMap<ApplicationRole, RemoveComment_RoleDto>();
        }
    }
}
