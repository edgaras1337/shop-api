using api.Dtos.CartControllerDtos;
using api.Models;
using AutoMapper;

namespace api.Dtos.WishlistControllerDtos
{
    public class RemoveWishlistItemResponse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public RWIR_ItemDto? Item { get; set; }
    }

    public class RWIR_ItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public RWIR_CategoryDto? Category { get; set; }
        public List<RWIR_ItemImageDto> Images { get; set; } = new List<RWIR_ItemImageDto>();
    }

    public class RWIR_CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class RWIR_ItemImageDto
    {
        public int Id { get; set; }
        public string ImageName { get; set; } = string.Empty;
        public string ImageSrc { get; set; } = string.Empty;
    }

    public class RemoveWishlistItemResponseProfiles : Profile
    {
        public RemoveWishlistItemResponseProfiles()
        {
            CreateMap<WishlistItem, RemoveWishlistItemResponse>();
            CreateMap<Item, RWIR_ItemDto>();
            CreateMap<Category, RWIR_CategoryDto>();
            CreateMap<ItemImage, RWIR_ItemImageDto>();
        }
    }
}
