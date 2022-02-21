using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class UserRole
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey("RoleId")]
        public int RoleId { get; set; }
        [Required]
        [ForeignKey("UserId")]
        public int UserId { get; set; }

        public Role? Role { get; set; }
        public User? User { get; set; }
    }
}
