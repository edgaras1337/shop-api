using api.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace api.Dtos.ItemControllerDtos
{
    public class CreateItemRequest
    {
        [Required]
        [MinLength(3)]
        public string Name { get; set; } = string.Empty;
        [MinLength(3)]
        public string Description { get; set; } = string.Empty;
        [Required]
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }
        [Required]
        [Range(0.0, double.MaxValue)]
        [Precision(10, 2)]
        public decimal Price { get; set; }
        public List<IFormFile> ItemImageFiles { get; set; } = new List<IFormFile>();
        [Required]
        public int CategoryId { get; set; }
    }

    public class CreateItemRequestProfiles : Profile
    {
        public CreateItemRequestProfiles()
        {
            CreateMap<CreateItemRequest, Item>();
        }
    }
}
