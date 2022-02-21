using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class Cart
    {
        [Key]
        [ForeignKey("UserId")]
        public int UserId { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }

        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
