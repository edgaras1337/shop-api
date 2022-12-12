using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Country { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public string ImageName { get; set; } = string.Empty;
        [NotMapped]
        public string ImageSource { get; set; } = string.Empty;

        public virtual List<ApplicationUserRole> UserRoles { get; set; } = new();
        public virtual Cart? Cart { get; set; }
        public virtual List<WishlistItem> WishlistItems { get; set; } = new();
        public virtual List<Comment> Comments { get; set; } = new();
        public virtual List<Purchase> Purchases { get; set; } = new();
    }
}
