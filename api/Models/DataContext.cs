using Microsoft.EntityFrameworkCore;

namespace api.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) 
            : base(options) 
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<UserRole> UserRoles => Set<UserRole>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Item> Items => Set<Item>();
        public DbSet<ItemImage> ItemImages => Set<ItemImage>();
        public DbSet<Comment> Comments => Set<Comment>();
        public DbSet<Cart> Carts => Set<Cart>();
        public DbSet<CartItem> CartItems => Set<CartItem>();
        public DbSet<WishlistItem> WishlistItems => Set<WishlistItem>();
        public DbSet<Purchase> Purchases => Set<Purchase>();
        public DbSet<PurchaseItem> PurchaseItems => Set<PurchaseItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(e => e.Email).IsUnique();
            modelBuilder.Entity<Role>().HasIndex(e => e.RoleName).IsUnique();
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, RoleName = "Administrator" },
                new Role { Id = 2, RoleName = "Customer" });


            modelBuilder.Entity<Category>().HasIndex(e => e.Name).IsUnique();
        }
    }
}
