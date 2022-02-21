using api.Models;
using AutoMapper;

namespace api.Dtos.CategoryControllerDtos
{
    public class CreateCategoryResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTimeOffset CreatedDate { get; set; }
        public string ImageName { get; set; } = string.Empty;
        public string ImageSource { get; set; } = string.Empty;
    }

    public class CreateCategoryResponseProfiles : Profile
    {
        public CreateCategoryResponseProfiles()
        {
            CreateMap<Category, CreateCategoryResponse>();
        }
    }
}
