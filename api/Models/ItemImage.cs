using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class ItemImage
    {
        [Key]
        public int Id { get; set; }
        public string ImageName { get; set; } = string.Empty;
        [NotMapped]
        public string ImageSource { get; set; } = string.Empty;
        [ForeignKey("ItemId")]
        public int ItemId { get; set; }

        public virtual Item? Item { get; set; }
    }
}
