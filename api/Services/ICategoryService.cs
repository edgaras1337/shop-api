using api.Dtos.CategoryControllerDtos;
using api.Models;

namespace api.Services
{
    public interface ICategoryService
    {
        Task<CreateCategoryResponse?> CreateAsync(CreateCategoryRequest request);
        Task<GetCategoryResponse?> GetCategoryByIdAsync(GetCategoryParams getParams);
        Task<List<GetCategoriesResponse>?> GetCategoriesAsync(GetCategoriesParams getParams);
        Task<UpdateCategoryResponse?> UpdateAsync(UpdateCategoryRequest request);
        Task DeleteByIdAsync(int id);
    }
}
