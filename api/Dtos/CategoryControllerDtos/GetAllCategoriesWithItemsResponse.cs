using api.Models;
using AutoMapper;
using Newtonsoft.Json;

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
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public GetAllCategoriesWithItemsResponse_ParentDto? Parent { get; set; }
        public List<GetAllCategoriesWithItemsResponse_ChildrenDto> Children { get; set; } = new();

        public bool ShouldSerializeChildren() =>
            Children.Count > 0;

        public bool ShouldSerializeItems() =>
            Children.Count == 0;
    }

    public class GetAllCategoriesWithItemsResponse_ParentDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ImageName { get; set; } = string.Empty;
        public string ImageSource { get; set; } = string.Empty;
        public DateTimeOffset CreatedDate { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public GetAllCategoriesWithItemsResponse_ParentDto? Parent { get; set; }
    }

    public class GetAllCategoriesWithItemsResponse_ChildrenDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ImageName { get; set; } = string.Empty;
        public string ImageSource { get; set; } = string.Empty;

        public List<GetAllCategoriesWithItemsResponse_ItemDto> Items { get; set; } = new();
        public List<GetAllCategoriesWithItemsResponse_ChildrenDto> Children { get; set; } = new();

        public bool ShouldSerializeItems() =>
            Children.Count == 0;

        public bool ShouldSerializeChildren() =>
            Children.Count > 0;
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
        public string ImageSource { get; set; } = string.Empty;
    }

    public class GetAllCategoriesWithItemsResponse_CommentDto
    {
        public int Id { get; set; }
        public string CommentText { get; set; } = string.Empty;
        public DateTimeOffset ModifiedDate { get; set; }
        public GetAllCategoriesWithItemsResponse_UserDto? User { get; set; }
    }

    public class GetAllCategoriesWithItemsResponse_UserDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ImageSource { get; set; } = string.Empty;
        public List<GetAllCategoriesWithItemsResponse_UserRoleDto> UserRoles { get; set; } = new();
    }

    public class GetAllCategoriesWithItemsResponse_UserRoleDto
    {
        public GetAllCategoriesWithItemsResponse_RoleDto? Role { get; set; }
    }

    public class GetAllCategoriesWithItemsResponse_RoleDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }

    public class GetAllCategoriesWithItemsResponseProfiles : Profile
    {
        public GetAllCategoriesWithItemsResponseProfiles()
        {
            CreateMap<Category, GetAllCategoriesWithItemsResponse>();
            CreateMap<Category, GetAllCategoriesWithItemsResponse_ParentDto>();
            CreateMap<Category, GetAllCategoriesWithItemsResponse_ChildrenDto>();
            CreateMap<Item, GetAllCategoriesWithItemsResponse_ItemDto>();
            CreateMap<ItemImage, GetAllCategoriesWithItemsResponse_ItemImageDto>();
            CreateMap<Comment, GetAllCategoriesWithItemsResponse_CommentDto>();
            CreateMap<ApplicationUser, GetAllCategoriesWithItemsResponse_UserDto>();
            CreateMap<ApplicationUserRole, GetAllCategoriesWithItemsResponse_UserRoleDto>();
            CreateMap<ApplicationRole, GetAllCategoriesWithItemsResponse_RoleDto>();
        }
    }
}
