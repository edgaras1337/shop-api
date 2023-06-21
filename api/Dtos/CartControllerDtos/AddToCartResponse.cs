using api.Models;
using AutoMapper;

namespace api.Dtos.CartControllerDtos
{
    public class AddToCartResponse
    {
        public int UserId { get; set; }
        public decimal TotalPrice { get; set; }

        public List<CartItemDto> CartItems { get; set; } = new();

        public class CartItemDto
        {
            public int Id { get; set; }
            public int Quantity { get; set; }

            public ItemDto? Item { get; set; }
            public class ItemDto
            {
                public int Id { get; set; }
                public string Name { get; set; } = string.Empty;
                public decimal Price { get; set; }
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
        }
    }

    public class AddToCartResponseProfiles : Profile
    {
        public AddToCartResponseProfiles()
        {
            CreateMap<Cart, AddToCartResponse>();
            CreateMap<CartItem, AddToCartResponse.CartItemDto>();
            CreateMap<Item, AddToCartResponse.CartItemDto.ItemDto>();
            CreateMap<Category, AddToCartResponse.CartItemDto.ItemDto.CategoryDto>();
            CreateMap<ItemImage, AddToCartResponse.CartItemDto.ItemDto.ItemImageDto>();
        }
    }
}
