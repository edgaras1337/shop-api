using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class ItemImage
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string ImageName { get; set; } = string.Empty;
        [NotMapped]
        public string ImageSrc { get; set; } = string.Empty;
        [Required]
        [ForeignKey("ItemId")]
        public int ItemId { get; set; }

        public virtual Item? Item { get; set; }
    }
}
