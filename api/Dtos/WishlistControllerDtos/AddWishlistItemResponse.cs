using api.Dtos.CartControllerDtos;
using api.Models;
using AutoMapper;

namespace api.Dtos.WishlistControllerDtos
{
    public class AddWishlistItemResponse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public AWIR_ItemDto? Item { get; set; }
    }

    public class AWIR_ItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public AWIR_CategoryDto? Category { get; set; }
        public List<AWIR_ItemImageDto> Images { get; set; } = new List<AWIR_ItemImageDto>();
    }

    public class AWIR_CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class AWIR_ItemImageDto
    {
        public int Id { get; set; }
        public string ImageName { get; set; } = string.Empty;
        public string ImageSrc { get; set; } = string.Empty;
    }

    public class AddWishlistItemResponseProfiles : Profile
    {
        public AddWishlistItemResponseProfiles()
        {
            CreateMap<WishlistItem, AddWishlistItemResponse>();
            CreateMap<Item, AWIR_ItemDto>();
            CreateMap<Category, AWIR_CategoryDto>();
            CreateMap<ItemImage, AWIR_ItemImageDto>();
        }
    }
}
