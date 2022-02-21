using api.Models;
using AutoMapper;

namespace api.Dtos.CartControllerDtos
{
    public class AddToCartResponse
    {
        public int UserId { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }

        public List<AddToCartRes_CartItemDto> CartItems { get; set; } = new();
    }

    public class AddToCartRes_CartItemDto
    {
        public int Id { get; set; }
        public int Quantity { get; set; }

        public AddToCartRes_ItemDto? Item { get; set; }
    }

    public class AddToCartRes_ItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public AddToCartRes_CategoryDto? Category { get; set; }
        public List<AddToCartRes_ItemImageDto> Images { get; set; } = new();
    }

    public class AddToCartRes_CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class AddToCartRes_ItemImageDto
    {
        public int Id { get; set; }
        public string ImageName { get; set; } = string.Empty;
        public string ImageSrc { get; set; } = string.Empty;
    }

    public class AddToCartResponseProfiles : Profile
    {
        public AddToCartResponseProfiles()
        {
            CreateMap<Cart, AddToCartResponse>();
            CreateMap<CartItem, AddToCartRes_CartItemDto>();
            CreateMap<Item, AddToCartRes_ItemDto>();
            CreateMap<Category, AddToCartRes_CategoryDto>();
            CreateMap<ItemImage, AddToCartRes_ItemImageDto>();
        }
    }
}
