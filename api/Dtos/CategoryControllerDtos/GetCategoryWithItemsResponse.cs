using api.Models;
using AutoMapper;

namespace api.Dtos.CategoryControllerDtos
{
    public class GetCategoryWithItemsResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public string ImageSource { get; set; } = string.Empty;

        public List<GetCategoryWithItemsResponse_ItemDto> Items { get; set; } = new();
    }

    public class GetCategoryWithItemsResponse_ItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public int? CategoryId { get; set; }

        public List<GetCategoryWithItemsResponse_ItemImageDto> Images { get; set; } = new();
        public List<GetCategoryWithItemsResponse_CommentDto> Comments { get; set; } = new();
    }

    public class GetCategoryWithItemsResponse_ItemImageDto
    {
        public int Id { get; set; }
        public string ImageName { get; set; } = string.Empty;
        public string ImageSrc { get; set; } = string.Empty;
    }

    public class GetCategoryWithItemsResponse_CommentDto
    {
        public int Id { get; set; }
        public string CommentText { get; set; } = string.Empty;
        public int UserId { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public GetCategoryWithItemsResponse_UserDto? User { get; set; }
    }

    public class GetCategoryWithItemsResponse_UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ImageSrc { get; set; } = string.Empty;
        public List<GetCategoryWithItemsResponse_UserRoleDto> UserRoles { get; set; } = new();
    }

    public class GetCategoryWithItemsResponse_UserRoleDto
    {
        public int Id { get; set; }
        public GetCategoryWithItemsResponse_RoleDto? Role { get; set; }
    }

    public class GetCategoryWithItemsResponse_RoleDto
    {
        public int Id { get; set; }
        public string RoleName { get; set; } = string.Empty;
    }

    public class GetCategoryWithItemsResponseProfiles : Profile
    {
        public GetCategoryWithItemsResponseProfiles()
        {
            CreateMap<Category, GetCategoryWithItemsResponse>();
            CreateMap<Item, GetCategoryWithItemsResponse_ItemDto>();
            CreateMap<ItemImage, GetCategoryWithItemsResponse_ItemImageDto>();
            CreateMap<Comment, GetCategoryWithItemsResponse_CommentDto>();
            CreateMap<User, GetCategoryWithItemsResponse_UserDto>();
            CreateMap<UserRole, GetCategoryWithItemsResponse_UserRoleDto>();
            CreateMap<Role, GetCategoryWithItemsResponse_RoleDto>();
        }
    }
}
