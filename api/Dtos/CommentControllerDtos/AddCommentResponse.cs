using api.Dtos.AuthControllerDtos;
using api.Models;
using AutoMapper;

namespace api.Dtos.CommentControllerDtos
{
    public class AddCommentResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public AddCommentResponse_CategoryDto? Category { get; set; }
        public List<AddCommentResponse_ItemImagesDto>? Images { get; set; }
        public List<AddCommentResponse_CommentDto> Comments { get; set; } = new List<AddCommentResponse_CommentDto>();
    }


    // navigation dtos
    public class AddCommentResponse_CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class AddCommentResponse_ItemImagesDto
    {
        public int Id { get; set; }
        public string ImageName { get; set; } = string.Empty;
        public string ImageSource { get; set; } = string.Empty;
    }

    public class AddCommentResponse_CommentDto
    {
        public int Id { get; set; }
        public string CommentText { get; set; } = string.Empty;
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public AddCommentResponse_UserDto? User { get; set; }
    }

    public class AddCommentResponse_UserDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public List<GetCurrentUser_UserRoleDto> UserRoles { get; set; } = new();
    }

    public class AddCommentResponse_UserRoleDto
    {
        public AddCommentResponse_RoleDto? Role { get; set; }
    }

    public class AddCommentResponse_RoleDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }


    // profiles for automapper
    public class AddCommentResponseProfiles : Profile
    {
        public AddCommentResponseProfiles()
        {
            CreateMap<Item, AddCommentResponse>();
            CreateMap<Category, AddCommentResponse_CategoryDto>();
            CreateMap<ItemImage, AddCommentResponse_ItemImagesDto>();
            CreateMap<Comment, AddCommentResponse_CommentDto>();
            CreateMap<ApplicationUser, AddCommentResponse_UserDto>();
            CreateMap<ApplicationUserRole, AddCommentResponse_UserRoleDto>();
            CreateMap<ApplicationRole, AddCommentResponse_RoleDto>();
        }
    }
}
