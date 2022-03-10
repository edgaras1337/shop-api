using api.Dtos.CartControllerDtos;
using api.Models;
using AutoMapper;

namespace api.Dtos.WishlistControllerDtos
{
    public class RemoveWishlistItemResponse
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public RemoveWishlistItemResponse_ItemDto? Item { get; set; }
    }

    public class RemoveWishlistItemResponse_ItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public RemoveWishlistItemResponse_CategoryDto? Category { get; set; }
        public List<RemoveWishlistItemResponse_ItemImageDto> Images { get; set; } = new();
    }

    public class RemoveWishlistItemResponse_CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class RemoveWishlistItemResponse_ItemImageDto
    {
        public int Id { get; set; }
        public string ImageName { get; set; } = string.Empty;
        public string ImageSource { get; set; } = string.Empty;
    }

    public class RemoveWishlistItemResponseProfiles : Profile
    {
        public RemoveWishlistItemResponseProfiles()
        {
            CreateMap<WishlistItem, RemoveWishlistItemResponse>();
            CreateMap<Item, RemoveWishlistItemResponse_ItemDto>();
            CreateMap<Category, RemoveWishlistItemResponse_CategoryDto>();
            CreateMap<ItemImage, RemoveWishlistItemResponse_ItemImageDto>();
        }
    }
}
