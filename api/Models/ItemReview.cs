using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class ItemReview
    {
        [Key]
        public int Id { get; set; }
        public string Comment { get; set; } = string.Empty;
        public int RatingCount { get; set; }
        [ForeignKey("ItemId")]
        public int ItemId { get; set; }
        [ForeignKey("UserId")]
        public int UserId { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }

        public virtual ApplicationUser? User { get; set; }
        public virtual Item? Item { get; set; }
    }
}
