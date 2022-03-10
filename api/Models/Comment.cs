using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public string CommentText { get; set; } = string.Empty;
        [ForeignKey("ItemId")]
        public int ItemId { get; set; }
        [ForeignKey("UserId")]
        public string UserId { get; set; } = string.Empty;
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }

        public virtual ApplicationUser? User { get; set; }
        public virtual Item? Item { get; set; }
    }
}
