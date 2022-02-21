using System;
using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public string ImageName { get; set; } = string.Empty;

        public virtual List<Item> Items { get; set; } = new List<Item>();
    }
}
