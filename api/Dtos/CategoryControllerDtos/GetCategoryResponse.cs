using api.Models;
using AutoMapper;
using Newtonsoft.Json;

namespace api.Dtos.CategoryControllerDtos
{
    public class GetCategoryResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public string ImageSource { get; set; } = string.Empty;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public GetCategoryResponse_ParentDto? Parent { get; set; }
        public List<GetCategoryResponse_ChildrenDto> Children { get; set; } = new();
    }

    public class GetCategoryResponse_ParentDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ImageName { get; set; } = string.Empty;
        public string ImageSource { get; set; } = string.Empty;
        public DateTimeOffset CreatedDate { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public GetCategoryResponse_ParentDto? Parent { get; set; }
    }

    public class GetCategoryResponse_ChildrenDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ImageName { get; set; } = string.Empty;
        public string ImageSource { get; set; } = string.Empty;
        public List<GetCategoryResponse_ChildrenDto> Children { get; set; } = new();
    }

    public class GetCategoryResponseProfiles : Profile
    {
        public GetCategoryResponseProfiles()
        {
            CreateMap<Category, GetCategoryResponse>();
            CreateMap<Category, GetCategoryResponse_ParentDto>();
            CreateMap<Category, GetCategoryResponse_ChildrenDto>();
        }
    }
}
