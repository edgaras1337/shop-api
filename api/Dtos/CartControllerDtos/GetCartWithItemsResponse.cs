using api.Models;
using AutoMapper;

namespace api.Dtos.CartControllerDtos
{
    public class GetCartWithItemsResponse
    {
        public int UserId { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }

        public List<GCWIR_CartItemDto> CartItems { get; set; } = new List<GCWIR_CartItemDto>();
    }

    public class GCWIR_CartItemDto
    {
        public int Id { get; set; }
        public int Quantity { get; set; }

        public GCWIR_ItemDto? Item { get; set; }
    }

    public class GCWIR_ItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public GCWIR_CategoryDto? Category { get; set; }
        public List<GCWIR_ItemImageDto> Images { get; set; } = new List<GCWIR_ItemImageDto>();
    }

    public class GCWIR_CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class GCWIR_ItemImageDto
    {
        public int Id { get; set; }
        public string ImageName { get; set; } = string.Empty;
        public string ImageSrc { get; set; } = string.Empty;
    }

    public class GetCartWithItemsResponseProfiles : Profile
    {
        public GetCartWithItemsResponseProfiles()
        {
            CreateMap<Cart, AddToCartResponse>();
            CreateMap<CartItem, GCWIR_CartItemDto>();
            CreateMap<Item, GCWIR_ItemDto>();
            CreateMap<Category, GCWIR_CategoryDto>();
            CreateMap<ItemImage, GCWIR_ItemImageDto>();
        }
    }
}
