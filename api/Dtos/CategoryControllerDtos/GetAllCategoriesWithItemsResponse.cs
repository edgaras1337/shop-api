using api.Models;
using AutoMapper;

namespace api.Dtos.CategoryControllerDtos
{
    public class GetAllCategoriesWithItemsResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public string ImageSource { get; set; } = string.Empty;

        public List<GetAllCategoriesWithItemsResponse_ItemDto> Items { get; set; } = new();
    }

    public class GetAllCategoriesWithItemsResponse_ItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public int? CategoryId { get; set; }

        public List<GetAllCategoriesWithItemsResponse_ItemImageDto> Images { get; set; } = new();
        public List<GetAllCategoriesWithItemsResponse_CommentDto> Comments { get; set; } = new();
    }

    public class GetAllCategoriesWithItemsResponse_ItemImageDto
    {
        public int Id { get; set; }
        public string ImageName { get; set; } = string.Empty;
        public string ImageSrc { get; set; } = string.Empty;
    }

    public class GetAllCategoriesWithItemsResponse_CommentDto
    {
        public int Id { get; set; }
        public string CommentText { get; set; } = string.Empty;
        public int UserId { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public GetAllCategoriesWithItemsResponse_UserDto? User { get; set; }
    }

    public class GetAllCategoriesWithItemsResponse_UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ImageSrc { get; set; } = string.Empty;
        public List<GetAllCategoriesWithItemsResponse_UserRoleDto> UserRoles { get; set; } = new();
    }

    public class GetAllCategoriesWithItemsResponse_UserRoleDto
    {
        public int Id { get; set; }
        public GetAllCategoriesWithItemsResponse_RoleDto? Role { get; set; }
    }

    public class GetAllCategoriesWithItemsResponse_RoleDto
    {
        public int Id { get; set; }
        public string RoleName { get; set; } = string.Empty;
    }

    public class GetAllCategoriesWithItemsResponseProfiles : Profile
    {
        public GetAllCategoriesWithItemsResponseProfiles()
        {
            CreateMap<Category, GetAllCategoriesWithItemsResponse>();
            CreateMap<Item, GetAllCategoriesWithItemsResponse_ItemDto>();
            CreateMap<ItemImage, GetAllCategoriesWithItemsResponse_ItemImageDto>();
            CreateMap<Comment, GetAllCategoriesWithItemsResponse_CommentDto>();
            CreateMap<User, GetAllCategoriesWithItemsResponse_UserDto>();
            CreateMap<UserRole, GetAllCategoriesWithItemsResponse_UserRoleDto>();
            CreateMap<Role, GetAllCategoriesWithItemsResponse_RoleDto>();
        }
    }
}
