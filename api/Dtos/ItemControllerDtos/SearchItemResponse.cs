using api.Models;
using AutoMapper;

namespace api.Dtos.ItemControllerDtos
{
    public class SearchItemResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public SIR_CategoryDto? Category { get; set; }
        public List<SIR_ItemImageDto>? Images { get; set; }
    }

    public class SIR_CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class SIR_ItemImageDto
    {
        public int Id { get; set; }
        public string ImageName { get; set; } = string.Empty;
        public string ImageSrc { get; set; } = string.Empty;
    }

    public class SearchItemResponseProfiles : Profile
    {
        public SearchItemResponseProfiles()
        {
            CreateMap<Item, GetAllItemsResponse>();
            CreateMap<Category, SIR_CategoryDto>();
            CreateMap<ItemImage, SIR_ItemImageDto>();
        }
    }
}
