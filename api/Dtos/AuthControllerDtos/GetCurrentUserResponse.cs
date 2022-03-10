using api.Models;
using AutoMapper;

namespace api.Dtos.AuthControllerDtos
{
    public class GetCurrentUserResponse
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string ImageSource { get; set; } = string.Empty;
        public List<GetCurrentUser_UserRoleDto> UserRoles { get; set; } = new();
        public GetCurrentUser_CartDto? Cart { get; set; }
        public List<GetCurrentUser_WishlistItemsDto> WishlistItems { get; set; } = new();
    }

    public class GetCurrentUser_UserRoleDto
    {
        public GetCurrentUser_RoleDto? Role { get; set; }
    }

    public class GetCurrentUser_RoleDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }

    public class GetCurrentUser_CartDto
    {
        public string UserId { get; set; } = string.Empty;
        public decimal TotalPrice { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }

        public List<GetCurrentUser_CartItemDto> CartItems { get; set; } = new();
    }

    public class GetCurrentUser_CartItemDto
    {
        public int Id { get; set; }
        public int Quantity { get; set; }

        public GetCurrentUser_ItemDto? Item { get; set; }
    }
    public class GetCurrentUser_WishlistItemsDto
    {
        public int Id { get; set; }
        public GetCurrentUser_ItemDto? Item { get; set; }
    }


    public class GetCurrentUser_ItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public GetCurrentUser_CategoryDto? Category { get; set; }
        public List<GetCurrentUser_ItemImageDto> Images { get; set; } = new();
    }

    public class GetCurrentUser_CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class GetCurrentUser_ItemImageDto
    {
        public int Id { get; set; }
        public string ImageName { get; set; } = string.Empty;
        public string ImageSource { get; set; } = string.Empty;
    }

    public class GetCurrentUserResponseProfiles : Profile
    {
        public GetCurrentUserResponseProfiles()
        {
            CreateMap<ApplicationUser, GetCurrentUserResponse>();
            CreateMap<ApplicationUserRole, GetCurrentUser_UserRoleDto>();
            CreateMap<ApplicationRole, GetCurrentUser_RoleDto>();
            CreateMap<Cart, GetCurrentUser_CartDto>();
            CreateMap<CartItem, GetCurrentUser_CartItemDto>();
            CreateMap<WishlistItem, GetCurrentUser_WishlistItemsDto>();
            CreateMap<Item, GetCurrentUser_ItemDto>();
            CreateMap<Category, GetCurrentUser_CategoryDto>();
            CreateMap<ItemImage, GetCurrentUser_ItemImageDto>();
        }
    }
}
