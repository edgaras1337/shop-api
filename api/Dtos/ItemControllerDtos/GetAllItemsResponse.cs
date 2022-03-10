using api.Models;
using AutoMapper;

namespace api.Dtos.ItemControllerDtos
{
    public class GetAllItemsResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public GetAllItemsResponse_CategoryDto? Category { get; set; }
        public List<GetAllItemsResponse_ItemImageDto>? Images { get; set; }
        public List<GetAllItemResponse_CommentDto> Comments { get; set; } = new();
    }

    public class GetAllItemsResponse_CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class GetAllItemsResponse_ItemImageDto
    {
        public int Id { get; set; }
        public string ImageName { get; set; } = string.Empty;
        public string ImageSource { get; set; } = string.Empty;
    }

    public class GetAllItemResponse_CommentDto
    {
        public int Id { get; set; }
        public string CommentText { get; set; } = string.Empty;
        public DateTimeOffset ModifiedDate { get; set; }
        public GetAllItemResponse_UserDto? User { get; set; }
    }

    public class GetAllItemResponse_UserDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ImageSource { get; set; } = string.Empty;
        public List<GetAllItemResponse_UserRoleDto> UserRoles { get; set; } = new();
    }

    public class GetAllItemResponse_UserRoleDto
    {
        public GetAllItemResponse_RoleDto? Role { get; set; }
    }

    public class GetAllItemResponse_RoleDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }

    public class GetAllItemsResponseProfiles : Profile
    {
        public GetAllItemsResponseProfiles()
        {
            CreateMap<Item, GetAllItemsResponse>();
            CreateMap<Category, GetAllItemsResponse_CategoryDto>();
            CreateMap<ItemImage, GetAllItemsResponse_ItemImageDto>();
            CreateMap<Comment, GetAllItemResponse_CommentDto>();
            CreateMap<ApplicationUser, GetAllItemResponse_UserDto>();
            CreateMap<ApplicationUserRole, GetAllItemResponse_UserRoleDto>();
            CreateMap<ApplicationRole, GetAllItemResponse_RoleDto>();
        }
    }
}
