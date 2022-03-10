using api.Dtos.ItemControllerDtos;

namespace api.Services
{
    public interface IItemService
    {
        Task<CreateItemResponse?> CreateItemAsync(CreateItemRequest request);
        Task<List<SearchItemResponse>> FindItemAsync(string searchKey);
        Task<GetItemResponse?> GetItemByIdAsync(int id);
        Task<List<GetAllItemsResponse>> GetAllItemsAsync();
        Task<UpdateItemResponse> UpdateItemAsync(UpdateItemRequest request);
        Task DeleteItemAsync(int id);
    }
}
