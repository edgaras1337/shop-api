using api.Dtos.CartControllerDtos;
using api.Models;
using AutoMapper;

namespace api.Dtos.WishlistControllerDtos
{
    public class AddWishlistItemResponse
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public DateTimeOffset AddedDate { get; set; }
        public AddWishlistItem_ItemDto? Item { get; set; }
    }

    public class AddWishlistItem_ItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public AddWishlistItem_CategoryDto? Category { get; set; }
        public List<AddWishlistItem_ItemImageDto> Images { get; set; } = new();
    }

    public class AddWishlistItem_CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class AddWishlistItem_ItemImageDto
    {
        public int Id { get; set; }
        public string ImageName { get; set; } = string.Empty;
        public string ImageSource { get; set; } = string.Empty;
    }

    public class AddWishlistItemResponseProfiles : Profile
    {
        public AddWishlistItemResponseProfiles()
        {
            CreateMap<WishlistItem, AddWishlistItemResponse>();
            CreateMap<Item, AddWishlistItem_ItemDto>();
            CreateMap<Category, AddWishlistItem_CategoryDto>();
            CreateMap<ItemImage, AddWishlistItem_ItemImageDto>();
        }
    }
}
