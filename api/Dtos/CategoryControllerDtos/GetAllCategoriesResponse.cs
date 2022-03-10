using api.Models;
using AutoMapper;
using Newtonsoft.Json;

namespace api.Dtos.CategoryControllerDtos
{
    public class GetAllCategoriesResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public string ImageSource { get; set; } = string.Empty;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public GetAllCategoriesResponse_ParentDto? Parent { get; set; }
        public List<GetAllCategoriesResponse_ChildrenDto> Children { get; set; } = new();
    }

    public class GetAllCategoriesResponse_ParentDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ImageName { get; set; } = string.Empty;
        public string ImageSource { get; set; } = string.Empty;
        public DateTimeOffset CreatedDate { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public GetAllCategoriesResponse_ParentDto? Parent { get; set; }
    }

    public class GetAllCategoriesResponse_ChildrenDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ImageName { get; set; } = string.Empty;
        public string ImageSource { get; set; } = string.Empty;
        public List<GetAllCategoriesResponse_ChildrenDto> Children { get; set; } = new();
    }

    public class GetAllCategoriesResponseProfiles : Profile
    {
        public GetAllCategoriesResponseProfiles()
        {
            CreateMap<Category, GetAllCategoriesResponse>();
            CreateMap<Category, GetAllCategoriesResponse_ParentDto>();
            CreateMap<Category, GetAllCategoriesResponse_ChildrenDto>();
        }
    }
}
