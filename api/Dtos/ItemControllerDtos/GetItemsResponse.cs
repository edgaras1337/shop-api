using api.Models;
using AutoMapper;

namespace api.Dtos.ItemControllerDtos
{
    public class GetItemsResponse
    {
        public IEnumerable<ItemDto> Items { get; set; } = new List<ItemDto>();

        public GetItemsResponse()
        {
        }

        public GetItemsResponse(IEnumerable<ItemDto> items)
        {
            Items = items;
        }

        public class ItemDto
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public int Quantity { get; set; }

            public List<ItemReviewDto>? Comments { get; set; }
            public ItemPriceDto? Price { get; set; }
            public CategoryDto? Category { get; set; }
            public List<ItemImageDto>? Images { get; set; }
            public List<ItemSpecDto>? ItemSpecs { get; set; }

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

            public class ItemSpecDto
            {
                public int Id { get; set; }
                public string SpecName { get; set; } = string.Empty;
                public string SpecValue { get; set; } = string.Empty;
            }

            public class ItemReviewDto
            {
                public int Id { get; set; }
                public string CommentText { get; set; } = string.Empty;
                public DateTimeOffset ModifiedDate { get; set; }
                public UserDto? User { get; set; }

                public class UserDto
                {
                    public string Id { get; set; } = string.Empty;
                    public string Name { get; set; } = string.Empty;
                    public string Surname { get; set; } = string.Empty;
                    public string Email { get; set; } = string.Empty;
                    public string ImageSource { get; set; } = string.Empty;
                    public List<UserRoleDto> UserRoles { get; set; } = new();

                    public class UserRoleDto
                    {
                        public RoleDto? Role { get; set; }

                        public class RoleDto
                        {
                            public string Id { get; set; } = string.Empty;
                            public string Name { get; set; } = string.Empty;
                        }
                    }
                }
            }
        }
    }

    public class GetItemsResponseProfiles : Profile
    {
        public GetItemsResponseProfiles()
        {
            CreateMap<Item, GetItemsResponse.ItemDto>();
            CreateMap<Category, GetItemsResponse.ItemDto.CategoryDto>();
            CreateMap<ItemImage, GetItemsResponse.ItemDto.ItemImageDto>();
            CreateMap<ItemPrice, GetItemsResponse.ItemDto.ItemPriceDto>();
            CreateMap<ItemSpec, GetItemsResponse.ItemDto.ItemSpecDto>();
            CreateMap<ItemReview, GetItemsResponse.ItemDto.ItemReviewDto>();
            CreateMap<ApplicationUser, GetItemsResponse.ItemDto.ItemReviewDto.UserDto>();
            CreateMap<ApplicationUserRole, GetItemsResponse.ItemDto.ItemReviewDto.UserDto.UserRoleDto>();
            CreateMap<ApplicationRole, GetItemsResponse.ItemDto.ItemReviewDto.UserDto.UserRoleDto.RoleDto>();
        }
    }
}
