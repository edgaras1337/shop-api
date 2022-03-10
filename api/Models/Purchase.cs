using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class Purchase
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("UserId")]
        public string? UserId { get; set; }
        [Precision(10, 2)]
        public decimal TotalPrice { get; set; }
        public DateTimeOffset PurchaseDate { get; set; }
        public bool IsDelivered { get; set; }

        public virtual ApplicationUser? User { get; set; }
        public virtual List<PurchaseItem> PurchaseItems { get; set; } = new();
        public virtual DeliveryAddress? DeliveryAddress { get; set; }

        public Purchase()
        {
            UserId = null;
            TotalPrice = 0;
            PurchaseDate = DateTimeOffset.UtcNow;
            IsDelivered = false;
        }

        public Purchase(string userId)
        {
            UserId = userId;
            TotalPrice = 0;
            PurchaseDate = DateTimeOffset.UtcNow;
            IsDelivered = false;
        }
    }
}
