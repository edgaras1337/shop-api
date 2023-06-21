using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api.Models
{
    public class SeedData
    {
        public static async Task InitializeRolesAndAdmin(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            using var scope = serviceProvider.CreateScope();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            string[] roles = new string[] { "Administrator", "Customer" };

            foreach (string role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new ApplicationRole { Name = role });
                }
            }

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var user = configuration.GetSection("PowerUser").Get<ApplicationUser>();

            if (!userManager.Users.Any(u => u.UserName == user.UserName))
            {
                var password = new PasswordHasher<ApplicationUser>();
                var hashed = password.HashPassword(user, user.PasswordHash);
                user.PasswordHash = hashed;

                await userManager.CreateAsync(user);

                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var cart = context.Carts
                    .SingleOrDefault(c => c.UserId == user.Id);

                if (cart == null)
                {
                    cart = new Cart(user.Id);

                    await context.Carts.AddAsync(cart);
                    await context.SaveChangesAsync();
                }

                await userManager.AddToRolesAsync(user, roles);
            }
        }
    }
}
