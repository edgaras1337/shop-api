using api.Dtos.WishlistControllerDtos;

namespace api.Services
{
    public interface IWishlistItemService
    {
        Task<List<AddWishlistItemResponse>> AddToWishlistAsync(int itemId);
        Task<List<RemoveWishlistItemResponse>> RemoveWishlistItemAsync(int itemId);
        Task<List<GetCurrentUserWishlistResponse>> GetCurrentUserWishlistAsync();
    }
}
