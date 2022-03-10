using api.Models;
using AutoMapper;

namespace api.Dtos.CartControllerDtos
{
    public class GetCartResponse
    {
        public string UserId { get; set; } = string.Empty;
        public decimal TotalPrice { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }

        public List<GetCartResponse_CartItemDto> CartItems { get; set; } = new();
    }

    public class GetCartResponse_CartItemDto
    {
        public int Id { get; set; }
        public int Quantity { get; set; }

        public GetCartResponse_ItemDto? Item { get; set; }
    }

    public class GetCartResponse_ItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public GetCartResponse_CategoryDto? Category { get; set; }
        public List<GetCartResponse_ItemImageDto> Images { get; set; } = new();
    }

    public class GetCartResponse_CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class GetCartResponse_ItemImageDto
    {
        public int Id { get; set; }
        public string ImageName { get; set; } = string.Empty;
        public string ImageSource { get; set; } = string.Empty;
    }

    public class GetCartWithItemsResponseProfiles : Profile
    {
        public GetCartWithItemsResponseProfiles()
        {
            CreateMap<Cart, GetCartResponse>();
            CreateMap<CartItem, GetCartResponse_CartItemDto>();
            CreateMap<Item, GetCartResponse_ItemDto>();
            CreateMap<Category, GetCartResponse_CategoryDto>();
            CreateMap<ItemImage, GetCartResponse_ItemImageDto>();
        }
    }
}
