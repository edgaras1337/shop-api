using api.Dtos.CartControllerDtos;
using api.Models;
using AutoMapper;

namespace api.Dtos.WishlistControllerDtos
{
    public class GetCurrentUserWishlistResponse
    {
        public int Id { get; set; }

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

    public class GetCurrentUserWishlistResponseProfiles : Profile
    {
        public GetCurrentUserWishlistResponseProfiles()
        {
            CreateMap<WishlistItem, GetCurrentUserWishlistResponse>();
            CreateMap<Item, GetCurrentUserWishlistResponse.ItemDto>();
            CreateMap<Category, GetCurrentUserWishlistResponse.ItemDto.CategoryDto>();
            CreateMap<ItemImage, GetCurrentUserWishlistResponse.ItemDto.ItemImageDto>();
        }
    }
}
