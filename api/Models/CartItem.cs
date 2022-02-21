using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }
        public int Quantity { get; set; }
        [ForeignKey("ItemId")]
        public int ItemId { get; set; }
        [ForeignKey("CartId")]
        public int CartId { get; set; }

        public Item? Item { get; set; }
        public Cart? Cart { get; set; }
    }
}
