using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class DeliveryAddress
    {
        [Key]
        [ForeignKey("PurchaseId")]
        public int PurchaseId { get; set; }
        public string Country { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public string StreetName { get; set; } = string.Empty;
        public string? HouseNumber { get; set; }
        public string? RoomNumber { get; set; }

        public Purchase? Purchase { get; set; }
    }
}
