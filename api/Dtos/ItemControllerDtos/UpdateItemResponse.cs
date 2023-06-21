using api.Models;
using AutoMapper;

namespace api.Dtos.ItemControllerDtos
{
    public class UpdateItemResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public CategoryDto? Category { get; set; }
        public List<ItemImageDto> Images { get; set; } = new();

        public class CategoryDto
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
        }

        public class ItemImageDto
        {
            public int Id { get; set; }
            public string ImageName { get; set; } = string.Empty;
            public string ImageSource { get; set; } = string.Empty;
        }
    }
    
    public class UpdateItemResponseProfiles : Profile
    {
        public UpdateItemResponseProfiles()
        {
            CreateMap<Item, UpdateItemResponse>();
            CreateMap<Category, UpdateItemResponse.CategoryDto>();
            CreateMap<ItemImage, UpdateItemResponse.ItemImageDto>();
        }
    }
}
