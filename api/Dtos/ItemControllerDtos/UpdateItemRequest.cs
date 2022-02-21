using api.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace api.Dtos.ItemControllerDtos
{
    public class UpdateItemRequest
    {
        public int Id { get; set; }
        [MinLength(3)]
        public string? Name { get; set; }
        [MinLength(3)]
        public string? Description { get; set; }
        [Range(0, int.MaxValue)]
        public int? Quantity { get; set; }
        [Range(0.0, double.MaxValue)]
        [Precision(10, 2)]
        public decimal? Price { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public List<IFormFile> ImageFilesToUpload { get; set; } = new List<IFormFile>();
        public List<int> ImageIdsToDelete { get; set; } = new List<int>();
        public int? CategoryId { get; set; }
    }

    public class UpdateItemRequestProfiles : Profile
    {
        public UpdateItemRequestProfiles()
        {
            CreateMap<UpdateItemRequest, Item>()
                .ForAllMembers(opts =>
                    opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
