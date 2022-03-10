using api.Dtos.CategoryControllerDtos;
using api.Models;

namespace api.Services
{
    public interface ICategoryService
    {
        Task<CreateCategoryResponse?> CreateAsync(CreateCategoryRequest request);
        Task<List<SearchCategoryWithItemsResponse>> FindCategoryAsync(string searchKey);
        Task<GetCategoryResponse?> GetCategoryByIdAsync(int id);
        Task<GetCategoryWithItemsResponse?> GetCategoryWithItemsByIdAsync(int id);
        Task<List<GetAllCategoriesWithItemsResponse>> GetAllCategoriesWithItemsAsync();
        Task<List<GetAllCategoriesResponse>> GetAllCategoriesAsync();
        Task<UpdateCategoryResponse?> UpdateAsync(UpdateCategoryRequest request);
        Task DeleteByIdAsync(int id);
    }
}
