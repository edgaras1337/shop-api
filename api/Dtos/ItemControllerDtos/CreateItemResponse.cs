using api.Models;
using AutoMapper;

namespace api.Dtos.ItemControllerDtos
{
    public class CreateItemResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public CreateItemResponse_CategoryDto? Category { get; set; }
        public List<CreateItemResponse_ItemImageDto>? Images { get; set; }
    }

    public class CreateItemResponse_CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class CreateItemResponse_ItemImageDto
    {
        public int Id { get; set; }
        public string ImageName { get; set; } = string.Empty;
        public string ImageSource { get; set; } = string.Empty;
    }

    public class CreateItemResponseProfiles : Profile
    {
        public CreateItemResponseProfiles()
        {
            CreateMap<Item, CreateItemResponse>();
            CreateMap<Category, CreateItemResponse_CategoryDto>();
            CreateMap<ItemImage, CreateItemResponse_ItemImageDto>();
        }
    }
}
