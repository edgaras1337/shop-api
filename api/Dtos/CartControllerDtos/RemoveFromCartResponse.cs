using api.Models;
using AutoMapper;

namespace api.Dtos.CartControllerDtos
{
    public class RemoveFromCartResponse
    {
        public int UserId { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }

        public List<RemoveFromCartResponse_CartItemDto> CartItems { get; set; } = new();
    }

    public class RemoveFromCartResponse_CartItemDto
    {
        public int Id { get; set; }
        public int Quantity { get; set; }

        public RemoveFromCartResponse_ItemDto? Item { get; set; }
    }

    public class RemoveFromCartResponse_ItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public RemoveFromCartResponse_CategoryDto? Category { get; set; }
        public List<RemoveFromCartResponse_ItemImageDto> Images { get; set; } = new();
    }

    public class RemoveFromCartResponse_CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class RemoveFromCartResponse_ItemImageDto
    {
        public int Id { get; set; }
        public string ImageName { get; set; } = string.Empty;
        public string ImageSource { get; set; } = string.Empty;
    }

    public class RemoveFromCartResponseProfiles : Profile
    {
        public RemoveFromCartResponseProfiles()
        {
            CreateMap<Cart, RemoveFromCartResponse>();
            CreateMap<CartItem, RemoveFromCartResponse_CartItemDto>();
            CreateMap<Item, RemoveFromCartResponse_ItemDto>();
            CreateMap<Category, RemoveFromCartResponse_CategoryDto>();
            CreateMap<ItemImage, RemoveFromCartResponse_ItemImageDto>();
        }
    }
}
