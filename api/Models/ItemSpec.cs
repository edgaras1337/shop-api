using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class ItemSpec
    {
        [Key]
        public int Id { get; set; }
        public string SpecName { get; set; } = string.Empty;
        public string SpecValue { get; set; } = string.Empty;

        [ForeignKey(nameof(ItemId))]
        public int ItemId { get; set; }
    
        public virtual Item? Item { get; set; }
    }
}
