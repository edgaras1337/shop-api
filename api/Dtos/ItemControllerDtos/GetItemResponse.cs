using api.Models;
using AutoMapper;

namespace api.Dtos.ItemControllerDtos
{
    public class GetItemResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public GetItemResponse_CategoryDto? Category { get; set; }
        public List<GetItemResponse_ItemImageDto>? Images { get; set; }
        public List<GetItemResponse_CommentDto>? Comments { get; set; }
    }

    public class GetItemResponse_CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class GetItemResponse_ItemImageDto
    {
        public int Id { get; set; }
        public string ImageName { get; set; } = string.Empty;
        public string ImageSrc { get; set; } = string.Empty;
    }

    public class GetItemResponse_CommentDto
    {
        public int Id { get; set; }
        public string CommentText { get; set; } = string.Empty;
        public int UserId { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public GetItemResponse_UserDto? User { get; set; }
    }

    public class GetItemResponse_UserDto 
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ImageSrc = string.Empty;
        public List<GetItemResponse_UserRoleDto> UserRoles { get; set; } = new();
    }

    public class GetItemResponse_UserRoleDto
    {
        public int Id { get; set; }
        public GetItemResponse_RoleDto? Role { get; set; }
    }

    public class GetItemResponse_RoleDto
    {
        public int Id { get; set; }
        public string RoleName { get; set; } = string.Empty;
    }

    public class GetItemResponseProfiles : Profile
    {
        public GetItemResponseProfiles()
        {
            CreateMap<Item, GetItemResponse>();
            CreateMap<Category, GetItemResponse_CategoryDto>();
            CreateMap<ItemImage, GetItemResponse_ItemImageDto>();
            CreateMap<Comment, GetItemResponse_CommentDto>();
            CreateMap<User, GetItemResponse_UserDto>();
            CreateMap<UserRole, GetItemResponse_UserRoleDto>();
            CreateMap<Role, GetItemResponse_RoleDto>();
        }
    }
}
