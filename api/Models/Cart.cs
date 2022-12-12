using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class Cart
    {
        [Key]
        [ForeignKey("UserId")]
        public int UserId { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal TotalPrice { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }

        public virtual List<CartItem> CartItems { get; set; } = new List<CartItem>();
        public virtual ApplicationUser? User { get; set; }

        public Cart(int userId)
        {
            UserId = userId;
            ModifiedDate = DateTimeOffset.UtcNow;
            TotalPrice = 0;
        }
    }
}
