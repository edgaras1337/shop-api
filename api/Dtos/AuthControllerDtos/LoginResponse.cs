using api.Models;
using AutoMapper;

namespace api.Dtos.AuthControllerDtos
{
    public class LoginResponse
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string ImageSource { get; set; } = string.Empty;
        public List<LoginResponse_UserRoleDto> UserRoles { get; set; } = new();
        public LoginResponse_Cart? Cart { get; set; }
        public List<LoginResponse_WishlistItemsDto>  WishlistItems { get; set; } = new();
    }

    public class LoginResponse_UserRoleDto
    {
        public LoginResponse_RoleDto? Role { get; set; }
    }

    public class LoginResponse_RoleDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }

    public class LoginResponse_Cart
    {
        public string UserId { get; set; } = string.Empty;
        public decimal TotalPrice { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }

        public List<LoginResponse_CartItemDto> CartItems { get; set; } = new();
    }

    public class LoginResponse_CartItemDto
    {
        public int Id { get; set; }
        public int Quantity { get; set; }

        public LoginResponse_ItemDto? Item { get; set; }
    }
    public class LoginResponse_WishlistItemsDto
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public LoginResponse_ItemDto? Item { get; set; }
    }


    public class LoginResponse_ItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public LoginResponse_CategoryDto? Category { get; set; }
        public List<LoginResponse_ItemImageDto> Images { get; set; } = new();
    }

    public class LoginResponse_CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class LoginResponse_ItemImageDto
    {
        public int Id { get; set; }
        public string ImageName { get; set; } = string.Empty;
        public string ImageSource { get; set; } = string.Empty;
    }

    public class LoginResponseProfiles : Profile
    {
        public LoginResponseProfiles()
        {
            CreateMap<ApplicationUser, LoginResponse>();
            CreateMap<ApplicationUserRole, LoginResponse_UserRoleDto>();
            CreateMap<ApplicationRole, LoginResponse_RoleDto>();
            CreateMap<Cart, LoginResponse_Cart>();
            CreateMap<CartItem, LoginResponse_CartItemDto>();
            CreateMap<WishlistItem, LoginResponse_WishlistItemsDto>();
            CreateMap<Item, LoginResponse_ItemDto>();
            CreateMap<Category, LoginResponse_CategoryDto>();
            CreateMap<ItemImage, LoginResponse_ItemImageDto>();
        }
    }
}
