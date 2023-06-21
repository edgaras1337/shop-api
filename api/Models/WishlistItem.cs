using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class WishlistItem
    {
        [Key]
        public int Id { get; init; }
        [ForeignKey("UserId")]
        public int UserId { get; init; }
        public DateTimeOffset AddedDate { get; set; }
        [ForeignKey("ItemId")]
        public int ItemId { get; set; }

        public virtual ApplicationUser? User { get; set; }
        public virtual Item? Item { get; set; }

        public WishlistItem()
        {
        }

        public WishlistItem(int userId, int itemId)
        {
            UserId = itemId;
            ItemId = itemId;
            AddedDate = DateTimeOffset.UtcNow;
        }
    }
}
