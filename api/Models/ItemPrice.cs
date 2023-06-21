using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class ItemPrice : Price
    {
        public DateTimeOffset Date { get; set; }
        public bool IsOnSale { get; set; }
        [ForeignKey(nameof(ItemId))]
        public int ItemId { get; set; }

        
        public virtual Item? Item { get; set; }
    }
}
