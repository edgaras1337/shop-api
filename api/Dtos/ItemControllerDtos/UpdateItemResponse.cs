using api.Models;
using AutoMapper;

namespace api.Dtos.ItemControllerDtos
{
    public class UpdateItemResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public UpdateItemResponse_CategoryDto? Category { get; set; }
        public List<UpdateItemResponse_ItemImageDto> Images { get; set; } = new();

        public class UpdateItemResponse_CategoryDto
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
        }

        public class UpdateItemResponse_ItemImageDto
        {
            public int Id { get; set; }
            public string ImageName { get; set; } = string.Empty;
            public string ImageSource { get; set; } = string.Empty;
        }

        public class GetItemResponseProfiles : Profile
        {
            public GetItemResponseProfiles()
            {
                CreateMap<Item, UpdateItemResponse>();
                CreateMap<Category, UpdateItemResponse_CategoryDto>();
                CreateMap<ItemImage, UpdateItemResponse_ItemImageDto>();
            }
        }
    }
}
