using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class PurchaseItem
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("ItemId")]
        public int? ItemId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Quantity { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }
        [ForeignKey("PurchaseId")]
        public int PurchaseId { get; set; }

        public virtual Purchase? Purchase { get; set; }
        public virtual Item? Item { get; set; }
    }
}
