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
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public bool IsFeatured { get; set; }
        [ForeignKey("CategoryId")]
        public int? CategoryId { get; set; }

        public virtual Category? Category { get; set; }
        public virtual List<ItemImage> Images { get; set; } = new();
        public virtual List<ItemReview> Reviews { get; set; } = new();
        public virtual List<PurchaseItem> PurchaseItems { get; set; } = new();
        public virtual List<ItemPrice> ItemPrices { get; set; } = new();
        public virtual List<ItemSpec> ItemSpecs { get; set; } = new();
    }
}
