using api.Models;
using AutoMapper;

namespace api.Dtos.UserControllerDtos
{
    public class UpdateUserResponse
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public DateTimeOffset DateOfBirth { get; set; }
        public string Email { get; set; } = string.Empty;
        public string ImageName { get; set; } = string.Empty;
        public string ImageSource { get; set; } = string.Empty;
        public List<UpdateUserResponse_UserRoleDto> UserRoles { get; set; } = new();
        public UpdateUserResponse_CartDto? Cart { get; set; }
        public List<UpdateUserResponse_WishlistItemsDto> WishlistItems { get; set; } = new();
    }

    public class UpdateUserResponse_UserRoleDto
    {
        public UpdateUserResponse_RoleDto? Role { get; set; }
    }

    public class UpdateUserResponse_RoleDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }

    public class UpdateUserResponse_CartDto
    {
        public string UserId { get; set; } = string.Empty;
        public decimal TotalPrice { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }

        public List<UpdateUserResponse_CartItemDto> CartItems { get; set; } = new();
    }

    public class UpdateUserResponse_CartItemDto
    {
        public int Id { get; set; }
        public int Quantity { get; set; }

        public UpdateUserResponse_ItemDto? Item { get; set; }
    }
    public class UpdateUserResponse_WishlistItemsDto
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public UpdateUserResponse_ItemDto? Item { get; set; }
    }


    public class UpdateUserResponse_ItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public UpdateUserResponse_CategoryDto? Category { get; set; }
        public List<UpdateUserResponse_ItemImageDto> Images { get; set; } = new();
    }

    public class UpdateUserResponse_CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class UpdateUserResponse_ItemImageDto
    {
        public int Id { get; set; }
        public string ImageName { get; set; } = string.Empty;
        public string ImageSource { get; set; } = string.Empty;
    }

    public class UpdateUserResponseProfiles : Profile
    {
        public UpdateUserResponseProfiles()
        {
            CreateMap<ApplicationUser, UpdateUserResponse>();
            CreateMap<ApplicationUserRole, UpdateUserResponse_UserRoleDto>();
            CreateMap<ApplicationRole, UpdateUserResponse_RoleDto>();
            CreateMap<Cart, UpdateUserResponse_CartDto>();
            CreateMap<CartItem, UpdateUserResponse_CartItemDto>();
            CreateMap<WishlistItem, UpdateUserResponse_WishlistItemsDto>();
            CreateMap<Item, UpdateUserResponse_ItemDto>();
            CreateMap<Category, UpdateUserResponse_CategoryDto>();
            CreateMap<ItemImage, UpdateUserResponse_ItemImageDto>();
        }
    }
}
