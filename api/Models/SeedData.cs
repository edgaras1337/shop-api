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
            var user = new ApplicationUser
            {
                Name = "edgaras",
                Surname = "franka",
                Email = "power.user@gmail.com",
                UserName = "power.user@gmail.com",
                PhoneNumber = "+00000000000",
                ImageName = configuration["ImagesConfiguration:DefaultUserImageName"],
                DateOfBirth = Convert.ToDateTime("2001-08-27"),
                Country = "Lithuania",
                City = "Vilnius",
                Address = "Fake street 900",
                ZipCode = "12345",
                SecurityStamp = new Guid().ToString(),
            };

            if (!userManager.Users.Any(u => u.UserName == user.UserName))
            {
                var password = new PasswordHasher<ApplicationUser>();
                var hashed = password.HashPassword(user, "abc123");
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
            }

            await userManager.AddToRolesAsync(user, new string[] { roles[0], roles[1] });
        }
    }
}
