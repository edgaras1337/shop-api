using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class ItemImageRepository : IItemImageRepository
    {
        private readonly ApplicationDbContext _context;

        public ItemImageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ItemImage> AddAsync(ItemImage itemImage)
        {
            await _context.ItemImages.AddAsync(itemImage);
            await _context.SaveChangesAsync();

            return itemImage;
        }
        public async Task<List<ItemImage>> AddRangeAsync(List<ItemImage> itemImage)
        {
            await _context.ItemImages.AddRangeAsync(itemImage);
            await _context.SaveChangesAsync();

            return itemImage;
        }
        public async Task<ItemImage?> GetByIdAsync(int id)
        {
            return await _context.ItemImages
                .SingleOrDefaultAsync(e => e.Id == id);
        }

        public async Task<List<ItemImage>> GetByItemIdAsync(int id)
        {
            return await _context.ItemImages
                .Where(e => e.ItemId == id)
                .ToListAsync();
        }

        public async Task DeleteImageAsync(ItemImage image)
        {
            _context.ItemImages.Remove(image);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteImagesAsync(List<ItemImage> images)
        {
            _context.ItemImages.RemoveRange(images);
            await _context.SaveChangesAsync();
        }
    }
}
