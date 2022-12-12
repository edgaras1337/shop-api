using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class ApplicationRole : IdentityRole<int>
    {
        public virtual List<ApplicationUserRole> UserRoles { get; set; } = new();
    }
}
