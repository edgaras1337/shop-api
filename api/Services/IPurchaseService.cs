using api.Dtos.PurchaseControllerDtos;

namespace api.Services
{
    public interface IPurchaseService
    {
        Task<CreatePurchaseResponse> CreatePurchaseAsync(CreatePurchaseRequest request);
        Task<List<GetCurrentUserPurchaseHistory>> GetCurrentUserPurchaseHistoryAsync();
        Task<List<GetAllPurchaseHistoryResponse>> GetAllPurchaseHistoryAsync();
        Task<List<GetUserPurchaseHistory>> GetUserPurchaseHistoryAsync(string userId);
    }
}
