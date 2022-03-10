using api.Models;
using AutoMapper;
using Newtonsoft.Json;

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
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public GetCategoryWithItemsResponse_ParentDto? Parent { get; set; }
        public List<GetCategoryWithItemsResponse_ChildrenDto> Children { get; set; } = new();

        public bool ShouldSerializeChildren() =>
            Children.Count > 0;

        public bool ShouldSerializeItems() =>
            Children.Count == 0;
    }

    public class GetCategoryWithItemsResponse_ParentDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ImageName { get; set; } = string.Empty;
        public string ImageSource { get; set; } = string.Empty;
        public DateTimeOffset CreatedDate { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public GetCategoryWithItemsResponse_ParentDto? Parent { get; set; }
    }

    public class GetCategoryWithItemsResponse_ChildrenDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ImageName { get; set; } = string.Empty;
        public string ImageSource { get; set; } = string.Empty;

        public List<GetCategoryWithItemsResponse_ItemDto> Items { get; set; } = new();
        public List<GetCategoryWithItemsResponse_ChildrenDto> Children { get; set; } = new();

        public bool ShouldSerializeItems() =>
            Children.Count == 0;

        public bool ShouldSerializeChildren() =>
            Children.Count > 0;
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
        public string ImageSource { get; set; } = string.Empty;
    }

    public class GetCategoryWithItemsResponse_CommentDto
    {
        public int Id { get; set; }
        public string CommentText { get; set; } = string.Empty;
        public DateTimeOffset ModifiedDate { get; set; }
        public GetCategoryWithItemsResponse_UserDto? User { get; set; }
    }

    public class GetCategoryWithItemsResponse_UserDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ImageSource { get; set; } = string.Empty;
        public List<GetCategoryWithItemsResponse_UserRoleDto> UserRoles { get; set; } = new();
    }

    public class GetCategoryWithItemsResponse_UserRoleDto
    {
        public GetCategoryWithItemsResponse_RoleDto? Role { get; set; }
    }

    public class GetCategoryWithItemsResponse_RoleDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }

    public class GetCategoryWithItemsResponseProfiles : Profile
    {
        public GetCategoryWithItemsResponseProfiles()
        {
            CreateMap<Category, GetCategoryWithItemsResponse>();
            CreateMap<Category, GetCategoryWithItemsResponse_ParentDto>();
            CreateMap<Category, GetCategoryWithItemsResponse_ChildrenDto>();
            CreateMap<Item, GetCategoryWithItemsResponse_ItemDto>();
            CreateMap<ItemImage, GetCategoryWithItemsResponse_ItemImageDto>();
            CreateMap<Comment, GetCategoryWithItemsResponse_CommentDto>();
            CreateMap<ApplicationUser, GetCategoryWithItemsResponse_UserDto>();
            CreateMap<ApplicationUserRole, GetCategoryWithItemsResponse_UserRoleDto>();
            CreateMap<ApplicationRole, GetCategoryWithItemsResponse_RoleDto>();
        }
    }
}
