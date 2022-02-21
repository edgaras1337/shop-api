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
        public UCR_CategoryDto? Category { get; set; }
        public List<UCR_ItemImagesDto>? Images { get; set; }
        public List<UCR_CommentDto> Comments { get; set; } = new List<UCR_CommentDto>();
    }


    // navigation dtos
    public class UCR_CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class UCR_ItemImagesDto
    {
        public int Id { get; set; }
        public string ImageName { get; set; } = string.Empty;
        public string ImageSrc { get; set; } = string.Empty;
    }

    public class UCR_CommentDto
    {
        public int Id { get; set; }
        public string CommentText { get; set; } = string.Empty;
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public UCR_UserDto? User { get; set; }
    }

    public class UCR_UserDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public List<UCR_UserRoleDto> UserRoles { get; set; } = new List<UCR_UserRoleDto>();
    }

    public class UCR_UserRoleDto
    {
        public int Id { get; set; }
        public UCR_RoleDto? Role { get; set; }
    }

    public class UCR_RoleDto
    {
        public int Id { get; set; }
        public string RoleName { get; set; } = string.Empty;
    }


    // profiles for automapper
    public class UpdateCommentResponseProfiles : Profile
    {
        public UpdateCommentResponseProfiles()
        {
            CreateMap<Item, UpdateCommentResponse>();
            CreateMap<Category, UCR_CategoryDto>();
            CreateMap<ItemImage, UCR_ItemImagesDto>();
            CreateMap<Comment, UCR_CommentDto>();
            CreateMap<User, UCR_UserDto>();
            CreateMap<UserRole, UCR_UserRoleDto>();
            CreateMap<Role, UCR_RoleDto>();
        }
    }
}
