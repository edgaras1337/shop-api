using api.Models;
using AutoMapper;

namespace api.Dtos.UserControllerDtos
{
    public class UpdateUserResponse
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

    public class UpdateUserResponseProfiles : Profile
    {
        public UpdateUserResponseProfiles()
        {
            CreateMap<ApplicationUser, UpdateUserResponse>();
            CreateMap<ApplicationUserRole, UpdateUserResponse.UserRoleDto>();
            CreateMap<ApplicationRole, UpdateUserResponse.UserRoleDto.RoleDto>();
            CreateMap<Cart, UpdateUserResponse.CartDto>();
            CreateMap<CartItem, UpdateUserResponse.CartDto.CartItemDto>();
            CreateMap<WishlistItem, UpdateUserResponse.WishlistItemsDto>();
            CreateMap<Item, UpdateUserResponse.ItemDto>();
            CreateMap<ItemPrice, UpdateUserResponse.ItemDto.ItemPriceDto>();
            CreateMap<Category, UpdateUserResponse.ItemDto.CategoryDto>();
            CreateMap<ItemImage, UpdateUserResponse.ItemDto.ItemImageDto>();
        }
    }
}
