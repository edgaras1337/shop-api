using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class PurchaseItem
    {
        [Key]
        [ForeignKey("PurchaseId")]
        public int PurchaseId { get; set; }
        [Precision(10, 2)]
        public int? ItemId { get; set; }
        [Required]
        public int Name { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal Price { get; set; }

        public Purchase? Purchase { get; set; }
        public Item? Item { get; set; }
    }
}
