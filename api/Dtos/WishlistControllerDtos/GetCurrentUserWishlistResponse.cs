using api.Dtos.CartControllerDtos;
using api.Models;
using AutoMapper;

namespace api.Dtos.WishlistControllerDtos
{
    public class GetCurrentUserWishlistResponse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTimeOffset AddedDate { get; set; }
        public GetCurrentUserWishlistResponse_ItemDto? Item { get; set; }
    }

    public class GetCurrentUserWishlistResponse_ItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public GetCurrentUserWishlistResponse_CategoryDto? Category { get; set; }
        public List<GetCurrentUserWishlistResponse_ItemImageDto> Images { get; set; } = new();
    }

    public class GetCurrentUserWishlistResponse_CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class GetCurrentUserWishlistResponse_ItemImageDto
    {
        public int Id { get; set; }
        public string ImageName { get; set; } = string.Empty;
        public string ImageSource { get; set; } = string.Empty;
    }

    public class GetCurrentUserWishlistResponseProfiles : Profile
    {
        public GetCurrentUserWishlistResponseProfiles()
        {
            CreateMap<WishlistItem, GetCurrentUserWishlistResponse>();
            CreateMap<Item, GetCurrentUserWishlistResponse_ItemDto>();
            CreateMap<Category, GetCurrentUserWishlistResponse_CategoryDto>();
            CreateMap<ItemImage, GetCurrentUserWishlistResponse_ItemImageDto>();
        }
    }
}
