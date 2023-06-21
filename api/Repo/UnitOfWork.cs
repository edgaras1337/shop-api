using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repo
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ApplicationDbContext _context;

        private GenericRepository<Item>? _itemRepository;
        private GenericRepository<ItemPrice>? _itemPriceRepository;
        private GenericRepository<Category>? _categoryRepository;
        private GenericRepository<Cart>? _cartRepository;
        private GenericRepository<WishlistItem>? _wishlistItemRepository;
        private GenericRepository<ItemReview>? _itemReviewRepository;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public GenericRepository<Item> ItemRepository => _itemRepository ??= new GenericRepository<Item>(_context);
        public GenericRepository<ItemPrice> ItemPriceRepository => _itemPriceRepository ??= new GenericRepository<ItemPrice>(_context);
        public GenericRepository<Category> CategoryRepository => _categoryRepository ??= new GenericRepository<Category>(_context);
        public GenericRepository<Cart> CartRepository => _cartRepository ??= new GenericRepository<Cart>(_context);
        public GenericRepository<WishlistItem> WishlistRepository => _wishlistItemRepository ??= new GenericRepository<WishlistItem>(_context);
        public GenericRepository<ItemReview> ItemReviewRepository => _itemReviewRepository ??= new GenericRepository<ItemReview>(_context);


        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
