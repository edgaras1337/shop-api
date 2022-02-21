using api.Models;
using AutoMapper;

namespace api.Dtos.CategoryControllerDtos
{
    public class GetCategoryResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public string ImageSource { get; set; } = string.Empty;
    }

    public class GetCategoryResponseProfiles : Profile
    {
        public GetCategoryResponseProfiles()
        {
            CreateMap<Category, GetCategoryResponse>();
        }
    }
}
