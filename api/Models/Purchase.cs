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
        public int? UserId { get; set; }
        [Required]
        [MinLength(1)]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Surname { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        [ForeignKey("CartId")]
        public int? CartId { get; set; }
        [Required]
        [Precision(10, 2)]
        public decimal TotalPrice { get; set; }
        [Required]
        public DateTimeOffset PurchaseDate { get; set; }
        [Required]
        public bool IsDelivered { get; set; }

        public User? User { get; set; }
        public Cart? Cart { get; set; }
        public List<PurchaseItem> PurchaseItems { get; set; } = new();
        public DeliveryAddress? DeliveryAddress { get; set; }
        public DeliveryContacts? DeliveryContacts { get; set; }
    }
}
