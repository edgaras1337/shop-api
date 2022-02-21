using api.Dtos.CartControllerDtos;
using api.Models;
using AutoMapper;

namespace api.Dtos.WishlistControllerDtos
{
    public class GetCurrentUserWishlistResponse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public GCUWR_ItemDto? Item { get; set; }
    }

    public class GCUWR_ItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public GCUWR_CategoryDto? Category { get; set; }
        public List<GCUWR_ItemImageDto> Images { get; set; } = new List<GCUWR_ItemImageDto>();
    }

    public class GCUWR_CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class GCUWR_ItemImageDto
    {
        public int Id { get; set; }
        public string ImageName { get; set; } = string.Empty;
        public string ImageSrc { get; set; } = string.Empty;
    }

    public class GetCurrentUserWishlistResponseProfiles : Profile
    {
        public GetCurrentUserWishlistResponseProfiles()
        {
            CreateMap<WishlistItem, GetCurrentUserWishlistResponse>();
            CreateMap<Item, GCUWR_ItemDto>();
            CreateMap<Category, GCUWR_CategoryDto>();
            CreateMap<ItemImage, GCUWR_ItemImageDto>();
        }
    }
}
