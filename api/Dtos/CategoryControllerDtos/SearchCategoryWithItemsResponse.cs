using api.Models;
using AutoMapper;

namespace api.Dtos.CategoryControllerDtos
{
    public class SearchCategoryWithItemsResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public string ImageSource { get; set; } = string.Empty;

        public List<SCWIR_ItemDto> Items { get; set; } = new List<SCWIR_ItemDto>();
    }

    public class SCWIR_ItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public int? CategoryId { get; set; }

        public List<SCWIR_ItemImageDto> Images { get; set; } = new List<SCWIR_ItemImageDto>();
    }

    public class SCWIR_ItemImageDto
    {
        public int Id { get; set; }
        public string ImageName { get; set; } = string.Empty;
        public string ImageSrc { get; set; } = string.Empty;
    }

    public class SearchCategoryWithItemsResponseProfiles : Profile
    {
        public SearchCategoryWithItemsResponseProfiles()
        {
            CreateMap<Category, SearchCategoryWithItemsResponse>();
            CreateMap<Item, SCWIR_ItemDto>();
            CreateMap<ItemImage, SCWIR_ItemImageDto>();
        }
    }
}
