using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class Item
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Required]
        public int? Quantity { get; set; }
        [Required]
        [Precision(10, 2)]
        public decimal? Price { get; set; }
        [Required]
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        [Required]
        [ForeignKey("CategoryId")]
        public int? CategoryId { get; set; }

        public virtual Category? Category { get; set; }
        public virtual List<ItemImage> Images { get; set; } = new List<ItemImage>();
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
