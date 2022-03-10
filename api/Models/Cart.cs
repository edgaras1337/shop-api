using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class Cart
    {
        [Key]
        [ForeignKey("UserId")]
        public string UserId { get; set; } = string.Empty;
        public decimal TotalPrice { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }

        public virtual List<CartItem> CartItems { get; set; } = new List<CartItem>();
        public virtual ApplicationUser? User { get; set; }

        public Cart(string userId)
        {
            UserId = userId;
            ModifiedDate = DateTimeOffset.UtcNow;
            TotalPrice = 0;
        }
    }
}
