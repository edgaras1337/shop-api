using api.Models;
using AutoMapper;

namespace api.Dtos.ItemControllerDtos
{
    public class SearchItemResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public SearchItemResponse_CategoryDto? Category { get; set; }
        public List<SearchItemResponse_ItemImageDto>? Images { get; set; }
    }

    public class SearchItemResponse_CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class SearchItemResponse_ItemImageDto
    {
        public int Id { get; set; }
        public string ImageName { get; set; } = string.Empty;
        public string ImageSource { get; set; } = string.Empty;
    }

    public class SearchItemResponse_CommentDto
    {
        public int Id { get; set; }
        public string CommentText { get; set; } = string.Empty;
        public DateTimeOffset ModifiedDate { get; set; }
        public GetItemResponse_UserDto? User { get; set; }
    }

    public class SearchItemResponse_UserDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ImageSource { get; set; } = string.Empty;
        public List<GetItemResponse_UserRoleDto> UserRoles { get; set; } = new();
    }

    public class SearchItemResponse_UserRoleDto
    {
        public GetItemResponse_RoleDto? Role { get; set; }
    }

    public class SearchItemResponse_RoleDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }

    public class SearchItemResponseProfiles : Profile
    {
        public SearchItemResponseProfiles()
        {
            CreateMap<Item, SearchItemResponse>();
            CreateMap<Category, SearchItemResponse_CategoryDto>();
            CreateMap<ItemImage, SearchItemResponse_ItemImageDto>();
            CreateMap<Comment, SearchItemResponse_CommentDto>();
            CreateMap<ApplicationUser, SearchItemResponse_UserDto>();
            CreateMap<ApplicationUserRole, SearchItemResponse_UserRoleDto>();
            CreateMap<ApplicationRole, SearchItemResponse_RoleDto>();
        }
    }
}
