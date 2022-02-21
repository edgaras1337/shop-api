using System.ComponentModel.DataAnnotations;

namespace api.Dtos.ItemControllerDtos
{
    public class SearchItemRequest
    {
        [Required]
        public string SearchKey { get; set; } = string.Empty;
    }
}
