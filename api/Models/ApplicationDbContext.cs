using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api.Models
{
    public class ApplicationDbContext
        : IdentityDbContext<
    ApplicationUser, ApplicationRole, int,
    IdentityUserClaim<int>, ApplicationUserRole, IdentityUserLogin<int>,
    IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options) 
        {
        }

        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Item> Items => Set<Item>();
        public DbSet<ItemPrice> ItemPrices => Set<ItemPrice>();
        public DbSet<ItemImage> ItemImages => Set<ItemImage>();
        public DbSet<ItemReview> ItemReviews => Set<ItemReview>();
        public DbSet<ItemSpec> ItemSpecs => Set<ItemSpec>();
        public DbSet<Cart> Carts => Set<Cart>();
        public DbSet<CartItem> CartItems => Set<CartItem>();
        public DbSet<WishlistItem> WishlistItems => Set<WishlistItem>();
        public DbSet<Purchase> Purchases => Set<Purchase>();
        public DbSet<PurchaseItem> PurchaseItems => Set<PurchaseItem>();
        public DbSet<DeliveryAddress> DeliveryAddresses => Set<DeliveryAddress>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            ModifyEntities(modelBuilder);
            ManageAutoIncludes(modelBuilder);
        }

        private static void ModifyEntities(ModelBuilder modelBuilder)
        {
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
                e.HasMany(e => e.ChildCategories)
                    .WithOne(e => e.ParentCategory)
                    .HasForeignKey(e => e.ParentCategoryId);

                e.HasIndex(e => e.Name)
                    .IsUnique();
            });

            modelBuilder.Entity<Purchase>()
                .Property(e => e.UserId)
                .IsRequired(false);

            modelBuilder.Entity<Item>(e =>
            {
                e.HasMany(e => e.PurchaseItems)
                    .WithOne(e => e.Item)
                    .OnDelete(DeleteBehavior.SetNull);
                
                e.HasMany(e => e.ItemPrices)
                    .WithOne(e => e.Item)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        private static void ManageAutoIncludes(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>()
                .Navigation(e => e.UserRoles)
                .AutoInclude();
            
            modelBuilder.Entity<ApplicationUserRole>()
                .Navigation(e => e.Role)
                .AutoInclude();
            
            modelBuilder.Entity<Item>()
                .Navigation(e => e.Images)
                .AutoInclude();

            modelBuilder.Entity<ItemReview>()
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
        }
    }
}
