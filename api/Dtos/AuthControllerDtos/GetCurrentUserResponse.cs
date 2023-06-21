using api.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace api.Dtos.AuthControllerDtos
{
    public class GetCurrentUserResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string ImageSource { get; set; } = string.Empty;
        public List<UserRoleDto> UserRoles { get; set; } = new();
        public CartDto? Cart { get; set; }
        public List<WishlistItemsDto> WishlistItems { get; set; } = new();

        public class UserRoleDto
        {
            public RoleDto? Role { get; set; }

            public class RoleDto
            {
                public int Id { get; set; }
                public string Name { get; set; } = string.Empty;
            }
        }

        public class CartDto
        {
            public int UserId { get; set; }
            public decimal TotalPrice { get; set; }
            public DateTimeOffset ModifiedDate { get; set; }

            public List<CartItemDto> CartItems { get; set; } = new();

            public class CartItemDto
            {
                public int Id { get; set; }
                public int Quantity { get; set; }

                public ItemDto? Item { get; set; }
            }
        }

        public class WishlistItemsDto
        {
            public int Id { get; set; }
            public ItemDto? Item { get; set; }
        }

        public class ItemDto
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public int Quantity { get; set; }

            public ItemPriceDto? Price { get; set; }
            public CategoryDto? Category { get; set; }
            public List<ItemImageDto>? Images { get; set; }

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

            public class ItemPriceDto
            {
                public int Id { get; set; }
                public decimal Price { get; set; }
                public bool IsOnSale { get; set; }
                public string Currency { get; set; } = string.Empty;
            }
        }
    }

    public class GetCurrentUserResponseProfiles : Profile
    {
        public GetCurrentUserResponseProfiles()
        {
            CreateMap<ApplicationUser, GetCurrentUserResponse>();
            CreateMap<ApplicationUserRole, GetCurrentUserResponse.UserRoleDto>();
            CreateMap<ApplicationRole, GetCurrentUserResponse.UserRoleDto.RoleDto>();
            CreateMap<Cart, GetCurrentUserResponse.CartDto>();
            CreateMap<CartItem, GetCurrentUserResponse.CartDto.CartItemDto>();
            CreateMap<WishlistItem, GetCurrentUserResponse.WishlistItemsDto>();
            CreateMap<Item, GetCurrentUserResponse.ItemDto>();
            CreateMap<ItemPrice, GetCurrentUserResponse.ItemDto.ItemPriceDto>();
            CreateMap<Category, GetCurrentUserResponse.ItemDto.CategoryDto>();
            CreateMap<ItemImage, GetCurrentUserResponse.ItemDto.ItemImageDto>();
        }
    }
}
