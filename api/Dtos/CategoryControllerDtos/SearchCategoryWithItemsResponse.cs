using api.Models;
using AutoMapper;
using Newtonsoft.Json;

namespace api.Dtos.CategoryControllerDtos
{
    public class SearchCategoryWithItemsResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public string ImageSource { get; set; } = string.Empty;

        public List<SearchCategoryWithItemsResponse_ItemDto> Items { get; set; } = new();
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public SearchCategoryWithItemsResponse_ParentDto? Parent { get; set; }
        public List<SearchCategoryWithItemsResponse_ChildrenDto> Children { get; set; } = new();

        public bool ShouldSerializeChildren() =>
            Children.Count > 0;

        public bool ShouldSerializeItems() =>
            Children.Count == 0;
    }

    public class SearchCategoryWithItemsResponse_ItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public int? CategoryId { get; set; }

        public List<SearchCategoryWithItemsResponse_ItemImageDto> Images { get; set; } = new();
        public List<SearchCategoryWithItemsResponse_CommentDto> Comments { get; set; } = new();
    }

    public class SearchCategoryWithItemsResponse_ParentDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ImageName { get; set; } = string.Empty;
        public string ImageSource { get; set; } = string.Empty;
        public DateTimeOffset CreatedDate { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public SearchCategoryWithItemsResponse_ParentDto? Parent { get; set; }
    }

    public class SearchCategoryWithItemsResponse_ChildrenDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ImageName { get; set; } = string.Empty;
        public string ImageSource { get; set; } = string.Empty;

        public List<SearchCategoryWithItemsResponse_ItemDto> Items { get; set; } = new();
        public List<SearchCategoryWithItemsResponse_ChildrenDto> Children { get; set; } = new();

        public bool ShouldSerializeItems() =>
            Children.Count == 0;

        public bool ShouldSerializeChildren() =>
            Children.Count > 0;
    }

    public class SearchCategoryWithItemsResponse_ItemImageDto
    {
        public int Id { get; set; }
        public string ImageName { get; set; } = string.Empty;
        public string ImageSource { get; set; } = string.Empty;
    }

    public class SearchCategoryWithItemsResponse_CommentDto
    {
        public int Id { get; set; }
        public string CommentText { get; set; } = string.Empty;
        public DateTimeOffset ModifiedDate { get; set; }
        public SearchCategoryWithItemsResponse_UserDto? User { get; set; }
    }

    public class SearchCategoryWithItemsResponse_UserDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ImageSource { get; set; } = string.Empty;
        public List<SearchCategoryWithItemsResponse_UserRoleDto> UserRoles { get; set; } = new();
    }

    public class SearchCategoryWithItemsResponse_UserRoleDto
    {
        public SearchCategoryWithItemsResponse_RoleDto? Role { get; set; }
    }

    public class SearchCategoryWithItemsResponse_RoleDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

    }

    public class SearchCategoryWithItemsResponseProfiles : Profile
    {
        public SearchCategoryWithItemsResponseProfiles()
        {
            CreateMap<Category, SearchCategoryWithItemsResponse>();
            CreateMap<Category, SearchCategoryWithItemsResponse_ParentDto>();
            CreateMap<Category, SearchCategoryWithItemsResponse_ChildrenDto>();
            CreateMap<Item, SearchCategoryWithItemsResponse_ItemDto>();
            CreateMap<ItemImage, SearchCategoryWithItemsResponse_ItemImageDto>();
            CreateMap<Comment, SearchCategoryWithItemsResponse_CommentDto>();
            CreateMap<ApplicationUser, SearchCategoryWithItemsResponse_UserDto>();
            CreateMap<ApplicationUserRole, SearchCategoryWithItemsResponse_UserRoleDto>();
            CreateMap<ApplicationRole, SearchCategoryWithItemsResponse_RoleDto>();
        }
    }
}
