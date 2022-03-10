using api.Models;
using AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace api.Dtos.CategoryControllerDtos
{
    public class CreateCategoryRequest
    {
        [Required]
        [MinLength(3)]
        public string Name { get; set; } = string.Empty;
        public int? ParentCategoryId { get; set; }
        public IFormFile? ImageFile { get; set; }
    }

    public class CreateCategoryRequestProfiles : Profile
    {
        public CreateCategoryRequestProfiles()
        {
            CreateMap<CreateCategoryRequest, Category>();
        }
    }
}
