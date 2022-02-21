using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace api.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Surname { get; set; } = string.Empty;
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string Email { get; set; } = string.Empty;
        public string ImageName { get; set; } = string.Empty;
        [NotMapped]
        public string ImageSrc { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;

        public List<UserRole> UserRoles { get; set; } = new();
        public Cart? Cart { get; set; }
        public List<WishlistItem> WishlistItems { get; set; } = new();
        public List<Comment> Comments = new();
    }
}
