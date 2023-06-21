namespace api.Dtos.ItemControllerDtos;

public class GetItemParams
{
    public int Id { get; set; }
    public bool IncludeReviews { get; set; }
    public bool IncludeImages { get; set; }
}