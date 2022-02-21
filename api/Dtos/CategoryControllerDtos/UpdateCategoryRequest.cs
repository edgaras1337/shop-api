using api.Models;
using AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace api.Dtos.CategoryControllerDtos
{
    public class UpdateCategoryRequest
    {
        [Required]
        public int Id { get; set; }
        [MinLength(3)]
        public string? Name { get; set; }
        public bool DeleteImage { get; set; } = false;
        public IFormFile? ImageToUpload { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
    }

    public class UpdateCategoryRequestProfiles : Profile
    {
        public UpdateCategoryRequestProfiles()
        {
            CreateMap<UpdateCategoryRequest, Category>()
                .ForAllMembers(opts =>
                    opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
