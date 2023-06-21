using api.Dtos.ItemControllerDtos;

namespace api.Services
{
    public interface IItemService
    {
        Task<CreateItemResponse?> CreateItemAsync(CreateItemRequest request);
        Task<GetItemsResponse> GetItems(GetItemsParams getParams);
        Task<GetItemResponse?> GetItemByIdAsync(GetItemParams getParams);
        Task<UpdateItemResponse> UpdateItemAsync(UpdateItemRequest request);
        Task DeleteItemAsync(int id);
    }
}
