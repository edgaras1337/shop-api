using api.Models;

namespace api.Data
{
    public interface IItemImageRepository
    {
        Task<ItemImage> AddAsync(ItemImage itemImage);
        Task<List<ItemImage>> AddRangeAsync(List<ItemImage> itemImage);
        Task<ItemImage?> GetByIdAsync(int id);
        Task<List<ItemImage>> GetByItemIdAsync(int id);
        Task DeleteImageAsync(ItemImage image);
        Task DeleteImagesAsync(List<ItemImage> images);
    }
}
