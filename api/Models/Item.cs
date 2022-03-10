using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class Item
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int? Quantity { get; set; }
        [Precision(10, 2)]
        public decimal? Price { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        [ForeignKey("CategoryId")]
        public int? CategoryId { get; set; }

        public virtual Category? Category { get; set; }
        public virtual List<ItemImage> Images { get; set; } = new List<ItemImage>();
        public virtual List<Comment> Comments { get; set; } = new List<Comment>();
        public virtual List<PurchaseItem> PurchaseItems { get; set; } = new();
    }
}
