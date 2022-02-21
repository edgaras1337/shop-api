namespace api.Dtos.WishlistControllerDtos
{
    public class ItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public CategoryDto? Category { get; set; }
        public List<ItemImageDto> Images { get; set; } = new List<ItemImageDto>();
    }
}
