using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class Price
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal PriceValue { get; set; }
    }
}
