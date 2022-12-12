using api.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace api.Dtos.AuthControllerDtos
{
    public class RegisterResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; } = string.Empty;
        public string ImageName { get; set; } = string.Empty;
        public string ImageSource { get; set; } = string.Empty;
        public List<RegisterResponse_UserRoleDto> UserRoles { get; set; } = new();
        public RegisterResponse_CartDto? Cart { get; set; }
        public List<RegisterResponse_WishlistItemsDto> WishlistItems { get; set; } = new();
    }

    public class RegisterResponse_UserRoleDto
    {
        public RegisterResponse_RoleDto? Role { get; set; }
    }

    public class RegisterResponse_RoleDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class RegisterResponse_CartDto
    {
        public int UserId { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }

        public List<RegisterResponse_CartItemDto> CartItems { get; set; } = new();
    }

    public class RegisterResponse_CartItemDto
    {
        public int Id { get; set; }
        public int Quantity { get; set; }

        public RegisterResponse_ItemDto? Item { get; set; }
    }
    public class RegisterResponse_WishlistItemsDto
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public RegisterResponse_ItemDto? Item { get; set; }
    }


    public class RegisterResponse_ItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public RegisterResponse_CategoryDto? Category { get; set; }
        public List<RegisterResponse_ItemImageDto> Images { get; set; } = new();
    }

    public class RegisterResponse_CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class RegisterResponse_ItemImageDto
    {
        public int Id { get; set; }
        public string ImageName { get; set; } = string.Empty;
        public string ImageSource { get; set; } = string.Empty;
    }

    public class RegisterResponseProfiles : Profile
    {
        public RegisterResponseProfiles()
        {
            CreateMap<ApplicationUser, RegisterResponse>();
            CreateMap<ApplicationUserRole, RegisterResponse_UserRoleDto>();
            CreateMap<ApplicationRole, RegisterResponse_RoleDto>();
            CreateMap<Cart, RegisterResponse_CartDto>();
            CreateMap<CartItem, RegisterResponse_CartItemDto>();
            CreateMap<WishlistItem, RegisterResponse_WishlistItemsDto>();
            CreateMap<Item, RegisterResponse_ItemDto>();
            CreateMap<Category, RegisterResponse_CategoryDto>();
            CreateMap<ItemImage, RegisterResponse_ItemImageDto>();
        }
    }
}
