using api.Models;
using AutoMapper;
using Newtonsoft.Json;

namespace api.Dtos.CategoryControllerDtos
{
    public class CreateCategoryResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTimeOffset CreatedDate { get; set; }
        public string ImageName { get; set; } = string.Empty;
        public string ImageSource { get; set; } = string.Empty;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public CreateCategoryResponse_ParentDto? Parent { get; set; }
    }

    public class CreateCategoryResponse_ParentDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ImageName { get; set; } = string.Empty;
        public string ImageSource { get; set; } = string.Empty;
        public DateTimeOffset CreatedDate { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public CreateCategoryResponse_ParentDto? Parent { get; set; }
    }

    public class CreateCategoryResponseProfiles : Profile
    {
        public CreateCategoryResponseProfiles()
        {
            CreateMap<Category, CreateCategoryResponse>();
            CreateMap<Category, CreateCategoryResponse_ParentDto>();
        }
    }
}
