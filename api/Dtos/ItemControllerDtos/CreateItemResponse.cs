using api.Models;
using AutoMapper;

namespace api.Dtos.ItemControllerDtos
{
    public class CreateItemResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public CIR_CategoryDto? Category { get; set; }
        public List<CIR_ItemImageDto>? Images { get; set; }
    }

    public class CIR_CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class CIR_ItemImageDto
    {
        public int Id { get; set; }
        public string ImageName { get; set; } = string.Empty;
        public string ImageSrc { get; set; } = string.Empty;
    }

    public class CreateItemResponseProfiles : Profile
    {
        public CreateItemResponseProfiles()
        {
            CreateMap<Item, CreateItemResponse>();
            CreateMap<Category, CIR_CategoryDto>();
            CreateMap<ItemImage, CIR_ItemImageDto>();
        }
    }
}
