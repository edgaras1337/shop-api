namespace api.Dtos.ItemControllerDtos
{
    public class GetItemsParams
    {
        public string? SearchKey { get; set; }
        public string? OrderBy { get; set; }
        public int? Count { get; set; }
        public int? Offset { get; set; }
        public bool IsFeatured { get; set; }
        public string Currency { get; set; } = "EUR";
        public bool IncludeReviews { get; set; }
        public bool IncludeDescription { get; set; }
        public bool IncludeImages { get; set; }
    }
}
