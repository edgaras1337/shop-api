using api.Models;
using AutoMapper;

namespace api.Dtos.CategoryControllerDtos
{
    public class GetAllCategoriesResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public string ImageSource { get; set; } = string.Empty;
    }

    public class GetAllCategoriesResponseProfiles : Profile
    {
        public GetAllCategoriesResponseProfiles()
        {
            CreateMap<Category, GetAllCategoriesResponse>();
        }
    }
}
