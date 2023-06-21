using api.Dtos.CartControllerDtos;
using api.Models;
using AutoMapper;

namespace api.Dtos.WishlistControllerDtos
{
    public class RemoveWishlistItemResponse
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

    public class RemoveWishlistItemResponseProfiles : Profile
    {
        public RemoveWishlistItemResponseProfiles()
        {
            CreateMap<WishlistItem, RemoveWishlistItemResponse>();
            CreateMap<Item, RemoveWishlistItemResponse.ItemDto>();
            CreateMap<Category, RemoveWishlistItemResponse.ItemDto.CategoryDto>();
            CreateMap<ItemImage, RemoveWishlistItemResponse.ItemDto.ItemImageDto>();
        }
    }
}
