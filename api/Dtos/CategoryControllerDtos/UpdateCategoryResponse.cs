using api.Models;
using AutoMapper;
using Newtonsoft.Json;

namespace api.Dtos.CategoryControllerDtos
{
    public class UpdateCategoryResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public string ImageSource { get; set; } = string.Empty;

        public List<UpdateCategoryResponse_ItemDto> Items { get; set; } = new();
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public UpdateCategoryResponse_ParentDto? Parent { get; set; }
        public List<UpdateCategoryResponse_ChildrenDto> Children { get; set; } = new();

        public bool ShouldSerializeChildren() =>
            Children.Count > 0;

        public bool ShouldSerializeItems() =>
            Children.Count == 0;
    }

    public class UpdateCategoryResponse_ParentDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ImageName { get; set; } = string.Empty;
        public string ImageSource { get; set; } = string.Empty;
        public DateTimeOffset CreatedDate { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public UpdateCategoryResponse_ParentDto? Parent { get; set; }
    }

    public class UpdateCategoryResponse_ChildrenDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ImageName { get; set; } = string.Empty;
        public string ImageSource { get; set; } = string.Empty;

        public List<UpdateCategoryResponse_ItemDto> Items { get; set; } = new();
        public List<UpdateCategoryResponse_ChildrenDto> Children { get; set; } = new();

        public bool ShouldSerializeItems() =>
            Children.Count == 0;

        public bool ShouldSerializeChildren() =>
            Children.Count > 0;
    }

    public class UpdateCategoryResponse_ItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public int? CategoryId { get; set; }

        public List<UpdateCategoryResponse_ItemImageDto> Images { get; set; } = new();
    }

    public class UpdateCategoryResponse_ItemImageDto
    {
        public int Id { get; set; }
        public string ImageName { get; set; } = string.Empty;
        public string ImageSource { get; set; } = string.Empty;
    }

    public class UpdateCategoryResponseProfiles : Profile
    {
        public UpdateCategoryResponseProfiles()
        {
            CreateMap<Category, UpdateCategoryResponse>();
            CreateMap<Category, UpdateCategoryResponse_ParentDto>();
            CreateMap<Category, UpdateCategoryResponse_ChildrenDto>();
            CreateMap<Item, UpdateCategoryResponse_ItemDto>();
            CreateMap<ItemImage, UpdateCategoryResponse_ItemImageDto>();
        }
    }
}
