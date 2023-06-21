using api.Dtos.CartControllerDtos;
using api.Extensions;
using api.Models;
using AutoMapper;

namespace api.Dtos.WishlistControllerDtos
{
    public class AddWishlistItemResponse
    {
        public int Id { get; set; }

        public ItemDto? Item { get; set; }
        public class ItemDto : ISinglePriceEntity
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

    public class AddWishlistItemResponseProfiles : Profile
    {
        public AddWishlistItemResponseProfiles()
        {
            CreateMap<WishlistItem, AddWishlistItemResponse>();
            CreateMap<Item, AddWishlistItemResponse.ItemDto>();
            CreateMap<Category, AddWishlistItemResponse.ItemDto.CategoryDto>();
            CreateMap<ItemImage, AddWishlistItemResponse.ItemDto.ItemImageDto>();
        }
    }
}
