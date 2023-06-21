using api.Models;

namespace api.Repo
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync();
        GenericRepository<Item> ItemRepository { get; }
        GenericRepository<ItemPrice> ItemPriceRepository { get; }
        GenericRepository<Category> CategoryRepository { get; }
        GenericRepository<Cart> CartRepository { get; }
        GenericRepository<WishlistItem> WishlistRepository { get; }
        GenericRepository<ItemReview> ItemReviewRepository { get; }
    }
}
