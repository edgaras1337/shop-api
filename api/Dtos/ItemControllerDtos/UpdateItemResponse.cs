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
        public UIR_CategoryDto? Category { get; set; }
        public List<UIR_ItemImageDto> Images { get; set; } = new List<UIR_ItemImageDto>();

        public class UIR_CategoryDto
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
        }

        public class UIR_ItemImageDto
        {
            public int Id { get; set; }
            public string ImageName { get; set; } = string.Empty;
            public string ImageSrc { get; set; } = string.Empty;
        }

        public class GetItemResponseProfiles : Profile
        {
            public GetItemResponseProfiles()
            {
                CreateMap<Item, GetAllItemsResponse>();
                CreateMap<Category, UIR_CategoryDto>();
                CreateMap<ItemImage, UIR_ItemImageDto>();
            }
        }
    }
}
