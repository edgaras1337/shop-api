using api.Models;
using AutoMapper;

namespace api.Dtos.CategoryControllerDtos
{
    public class UpdateCategoryResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public string ImageSource { get; set; } = string.Empty;

        public List<UCR_ItemDto> Items { get; set; } = new List<UCR_ItemDto>();
    }

    public class UCR_ItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public int? CategoryId { get; set; }

        public List<UCR_ItemImageDto> Images { get; set; } = new List<UCR_ItemImageDto>();
    }

    public class UCR_ItemImageDto
    {
        public int Id { get; set; }
        public string ImageName { get; set; } = string.Empty;
        public string ImageSrc { get; set; } = string.Empty;
    }

    public class UpdateCategoryResponseProfiles : Profile
    {
        public UpdateCategoryResponseProfiles()
        {
            CreateMap<Category, UpdateCategoryResponse>();
            CreateMap<Item, UCR_ItemDto>();
            CreateMap<ItemImage, UCR_ItemImageDto>();
        }
    }
}
