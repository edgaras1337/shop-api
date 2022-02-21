using api.Models;
using AutoMapper;

namespace api.Dtos.CartControllerDtos
{
    public class RemoveFromCartResponse
    {
        public int UserId { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }

        public List<RFCR_CartItemDto> CartItems { get; set; } = new List<RFCR_CartItemDto>();
    }

    public class RFCR_CartItemDto
    {
        public int Id { get; set; }
        public int Quantity { get; set; }

        public RFCR_ItemDto? Item { get; set; }
    }

    public class RFCR_ItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public RFCR_CategoryDto? Category { get; set; }
        public List<RFCR_ItemImageDto> Images { get; set; } = new List<RFCR_ItemImageDto>();
    }

    public class RFCR_CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class RFCR_ItemImageDto
    {
        public int Id { get; set; }
        public string ImageName { get; set; } = string.Empty;
        public string ImageSrc { get; set; } = string.Empty;
    }

    public class RemoveFromCartResponseProfiles : Profile
    {
        public RemoveFromCartResponseProfiles()
        {
            CreateMap<Cart, RemoveFromCartResponse>();
            CreateMap<CartItem, RFCR_CartItemDto>();
            CreateMap<Item, RFCR_ItemDto>();
            CreateMap<Category, RFCR_CategoryDto>();
            CreateMap<ItemImage, RFCR_ItemImageDto>();
        }
    }
}
