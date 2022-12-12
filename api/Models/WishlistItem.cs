using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class WishlistItem
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("UserId")]
        public int UserId { get; set; }
        public DateTimeOffset AddedDate { get; set; }
        [ForeignKey("ItemId")]
        public int ItemId { get; set; }

        public virtual ApplicationUser? User { get; set; }
        public virtual Item? Item { get; set; }
    }
}
