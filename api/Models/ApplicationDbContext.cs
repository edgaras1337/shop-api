using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api.Models
{
    public class ApplicationDbContext
        : IdentityDbContext//<ApplicationUser, ApplicationRole, string>
        <
    ApplicationUser, ApplicationRole, string,
    IdentityUserClaim<string>, ApplicationUserRole, IdentityUserLogin<string>,
    IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options) 
        {
        }

        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Item> Items => Set<Item>();
        public DbSet<ItemImage> ItemImages => Set<ItemImage>();
        public DbSet<Comment> Comments => Set<Comment>();
        public DbSet<Cart> Carts => Set<Cart>();
        public DbSet<CartItem> CartItems => Set<CartItem>();
        public DbSet<WishlistItem> WishlistItems => Set<WishlistItem>();
        public DbSet<Purchase> Purchases => Set<Purchase>();
        public DbSet<PurchaseItem> PurchaseItems => Set<PurchaseItem>();
        public DbSet<DeliveryAddress> DeliveryAddresses => Set<DeliveryAddress>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>(e =>
            {
                e.HasMany(e => e.UserRoles)
                    .WithOne(e => e.User)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();

                e.HasOne(e => e.Cart)
                    .WithOne(e => e.User)
                    .IsRequired();

                e.HasMany(e => e.WishlistItems)
                    .WithOne(e => e.User)
                    .HasForeignKey(e => e.UserId)
                    .IsRequired();

                e.HasMany(e => e.Comments)
                    .WithOne(e => e.User)
                    .HasForeignKey(e => e.UserId)
                    .IsRequired();

                e.HasMany(e => e.Purchases)
                    .WithOne(e => e.User)
                    .HasForeignKey(e => e.UserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.SetNull);


            });

            modelBuilder.Entity<ApplicationRole>(e =>
            {
                e.HasMany(e => e.UserRoles)
                    .WithOne(e => e.Role)
                    .HasForeignKey(e => e.RoleId)
                    .IsRequired();
            });

            modelBuilder.Entity<Category>(e => {
                e.HasMany(e => e.Children)
                .WithOne(e => e.Parent)
                .HasForeignKey(e => e.ParentCategoryId);

                e.HasIndex(e => e.Name)
                    .IsUnique();
            });

            modelBuilder.Entity<Purchase>()
                .Property(e => e.UserId)
                .IsRequired(false);

            modelBuilder.Entity<Item>()
                .HasMany(e => e.PurchaseItems)
                .WithOne(e => e.Item)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<ApplicationUser>()
                .Navigation(e => e.UserRoles)
                .AutoInclude();
            
            modelBuilder.Entity<ApplicationUserRole>()
                .Navigation(e => e.Role)
                .AutoInclude();
            
            modelBuilder.Entity<Item>()
                .Navigation(e => e.Images)
                .AutoInclude();

            modelBuilder.Entity<Comment>()
                .Navigation(e => e.User)
                .AutoInclude();

            modelBuilder.Entity<WishlistItem>()
                .Navigation(e => e.Item)
                .AutoInclude();
            
            modelBuilder.Entity<Cart>()
                .Navigation(e => e.CartItems)
                .AutoInclude();
            
            modelBuilder.Entity<CartItem>()
                .Navigation(e => e.Item)
                .AutoInclude();

            modelBuilder.Entity<Purchase>()
                .Navigation(e => e.PurchaseItems)
                .AutoInclude();

            modelBuilder.Entity<Purchase>()
                .Navigation(e => e.DeliveryAddress)
                .AutoInclude();
        }
    }
}
