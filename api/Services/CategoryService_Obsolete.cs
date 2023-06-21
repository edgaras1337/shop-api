using api.CustomExceptions;
using api.Dtos.CategoryControllerDtos;
using api.Helpers;
using api.Models;
using api.Repo;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Xml.Linq;

namespace api.Services
{
    // [Obsolete("Not complete.")]
    // public class CategoryService_Obsolete : ICategoryService
    // {
    //     //private readonly ICategoryRepository _categoryRepository;
    //     private readonly IMapper _mapper;
    //     private readonly IImageService _imageService;
    //     private readonly IConfiguration _config;
    //     private readonly IUnitOfWork _unitOfWork;
    //
    //     public CategoryService_Obsolete(
    //         //ICategoryRepository categoryRepository, 
    //         IMapper mapper, 
    //         IImageService imageService,
    //         IConfiguration config,
    //         IUnitOfWork unitOfWork)
    //     {
    //         //_categoryRepository = categoryRepository;
    //         _mapper = mapper;
    //         _imageService = imageService;
    //         _config = config;
    //         _unitOfWork = unitOfWork;
    //     }
    //
    //     public async Task<CreateCategoryResponse?> CreateAsync(CreateCategoryRequest dto)
    //     {
    //         // return null if name is not unique
    //         if (!await IsNameUniqueAsync(dto.Name))
    //         {
    //             throw new DuplicateDataException();
    //         }
    //
    //         var category = _mapper.Map<Category>(dto);
    //         category.CreatedDate = DateTimeOffset.UtcNow;
    //         category.ModifiedDate = category.CreatedDate;
    //
    //         category.ImageName = await _imageService.SaveImageAsync(dto.ImageFile) ??
    //             _config["ImagesConfiguration:DefaultCategoryImageName"];
    //
    //         if (dto.ParentCategoryId != null)
    //         {
    //             var parent = await _unitOfWork.CategoryRepository.GetByIdAsync(dto.ParentCategoryId.Value) ?? throw new ObjectNotFoundException();
    //             category.ParentCategory = parent;
    //         }
    //
    //         // add the category with unique name to db
    //         await _unitOfWork.CategoryRepository.AddAsync(category);
    //         await _unitOfWork.SaveChangesAsync();
    //
    //         return _mapper.Map<CreateCategoryResponse>(category);
    //     }
    //
    //     public async Task<GetCategoryResponse?> GetCategoryByIdAsync(GetCategoryParams getParams)
    //     {
    //         var categoriesQuery = _unitOfWork.CategoryRepository.GetAllQuery()
    //             .Where(e => e.Id == getParams.Id);
    //
    //         if (getParams.IncludeItems)
    //         {
    //             categoriesQuery = categoriesQuery.Include(e => e.Items);
    //         }
    //
    //         Category? category;
    //         if (getParams.IncludeChildren)
    //         {
    //             categoriesQuery = categoriesQuery.Include(e => e.ChildCategories);
    //             if (getParams.IncludeItems)
    //             {
    //                 categoriesQuery = categoriesQuery.Include(e => e.Items);
    //             }
    //             category = await categoriesQuery.SingleOrDefaultAsync();
    //             if (category == null)
    //             {
    //                 return null;
    //             }
    //
    //             if (category.ChildCategories != null)
    //             {
    //                 await MapChildrenAsync(category.ChildCategories, getParams.IncludeItems ? (query) => query.Include(e => e.Items) : null);
    //             }
    //         }
    //         else
    //         {
    //             category = await categoriesQuery.SingleAsync();
    //         }
    //
    //         return _mapper.Map<GetCategoryResponse>(category);
    //     }
    //
    //
    //
    //     public async Task<List<GetCategoriesResponse>?> GetCategoriesAsync(GetCategoriesParams getParams)
    //     {
    //         var categoriesQuery = _unitOfWork.CategoryRepository.GetAllQuery();
    //
    //         if (getParams.IncludeItems)
    //         {
    //             categoriesQuery = categoriesQuery.Include(e => e.Items);
    //         }
    //
    //         var list = await categoriesQuery.ToListAsync();
    //         foreach (var item in list.ToList())
    //         {
    //             if (item.ParentCategoryId != null)
    //             {
    //                 list.Remove(item);
    //             }
    //         }
    //
    //         return _mapper.Map<List<GetCategoriesResponse>>(list);
    //     }
    //
    //     private async Task MapChildrenAsync(List<Category> categories, Action<IQueryable<Category>>? onBeforeGetData = null)
    //     {
    //         foreach (var category in categories)
    //         {
    //             IQueryable<Category> query = _unitOfWork.CategoryRepository.GetAllQuery()
    //                 .Where(e => e.ParentCategoryId == category.Id)
    //                 .Include(e => e.ChildCategories);
    //
    //             onBeforeGetData?.Invoke(query);
    //
    //             category.ChildCategories = await query.ToListAsync();
    //
    //             if (category.ChildCategories?.Any() ?? false)
    //             {
    //                 await MapChildrenAsync(category.ChildCategories, onBeforeGetData);
    //             }
    //         }
    //     }
    //
    //     //private async Task MapChildrenAsync(List<Category> categories, bool includeItems)
    //     //{
    //     //    foreach (var category in categories)
    //     //    {
    //     //        IQueryable<Category> query = _unitOfWork.CategoryRepository.GetAllQuery()
    //     //            .Where(e => e.ParentCategoryId == category.Id)
    //     //            .Include(e => e.ChildCategories);
    //
    //
    //     //        onBeforeGetData?.Invoke(query);
    //
    //     //        category.ChildCategories = await query.ToListAsync();
    //
    //     //        if (category.ChildCategories?.Any() ?? false)
    //     //        {
    //     //            await MapChildrenAsync(category.ChildCategories, onBeforeGetData);
    //     //        }
    //     //    }
    //     //}
    //
    //     private void LoopThroughNestedList(List<Category> nestedList, Action<Category> exec)
    //     {
    //         foreach (var item in nestedList)
    //         {
    //             exec(item);
    //             if (item.ChildCategories != null && item.ChildCategories.Any())
    //             {
    //                 LoopThroughNestedList(item.ChildCategories, exec);
    //             }
    //         }
    //     }
    //
    //     //private Task GetCategories(int id, string searchKey)
    //
    //
    //     //public async Task<List<GetCategoryTreeResponse>> GetCategoryTreeAsync()
    //     //{
    //     //    var categories = await _categoryRepository.GetAllAsync();
    //     //    if (categories == null || categories.Count == 0)
    //     //    {
    //     //        return null;
    //     //    }
    //     //    categories = categories.Where(c => c.Parent == null).ToList();
    //     //    var result = await MapCategories<GetCategoryTreeResponse>(categories);
    //     //    return result;
    //     //}
    //
    //     //public async Task<List<GetAllCategoriesWithItemsResponse>> GetAllCategoriesWithItemsAsync()
    //     //{
    //     //    var categories = await _categoryRepository.GetAllWithItemsAsync();
    //     //    var result = await MapCategories<GetAllCategoriesWithItemsResponse>(categories);
    //     //    return result;
    //     //}
    //
    //     public async Task<UpdateCategoryResponse?> UpdateAsync(UpdateCategoryRequest dto)
    //     {
    //         // category photo update
    //         var category = await _unitOfWork.CategoryRepository.GetAllQuery()
    //             .Include(e => e.Items)
    //             .SingleOrDefaultAsync(e => e.Id == dto.Id) ?? throw new ObjectNotFoundException();
    //         if (dto.Name != null && !await IsNameUniqueAsync(dto.Name))
    //         {
    //             throw new DuplicateNameException();
    //         }
    //
    //         _mapper.Map(dto, category);
    //
    //         category.ModifiedDate = DateTimeOffset.UtcNow;
    //
    //         // update image
    //         var defaultImageName = _config["ImagesConfiguration:DefaultCategoryImageName"];
    //         if (dto.DeleteImage)
    //         {
    //             // delete image only when its not the default one
    //             // when deleted set the default image
    //             if (category.ImageName != defaultImageName)
    //             {
    //                 await _imageService.DeleteImageFileAsync(category.ImageName);
    //                 category.ImageName = defaultImageName;
    //             }
    //         }
    //         if (dto.ImageToUpload != null)
    //         {
    //             // save the selected image, if failed, set default image
    //             category.ImageName = await _imageService.SaveImageAsync(dto.ImageToUpload) ?? defaultImageName;
    //         }
    //
    //         await _unitOfWork.SaveChangesAsync();
    //
    //         await _imageService.LoadImagesAsync(category);
    //         return _mapper.Map<UpdateCategoryResponse>(category);
    //     }
    //
    //     public async Task DeleteByIdAsync(int id)
    //     {
    //         var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id) ?? throw new ObjectNotFoundException();
    //
    //         // Cannot delete if items still exist.
    //         if (category.Items.Count > 0)
    //         {
    //             throw new RelatedDataException();
    //         }
    //
    //         // delete image file
    //         await _imageService.DeleteImageFileAsync(category.ImageName);
    //
    //         // delete the category
    //         _unitOfWork.CategoryRepository.Delete(category);
    //         await _unitOfWork.SaveChangesAsync();
    //     }
    //
    //
    //     // helpers
    //     private async Task<bool> IsNameUniqueAsync(string name)
    //     {
    //         return (await _unitOfWork.CategoryRepository.GetAllQuery().SingleOrDefaultAsync(e => e.Name == name)) == null;
    //     }
    //
    //     //private async Task<List<T>> MapCategories<T>(List<Category>? categories)
    //     //{
    //     //    var dtoList = new List<T>();
    //
    //     //    categories?.ForEach(async category =>
    //     //    {
    //     //        // add item and user images
    //     //        category = await category.WithImagesAsync(_imageService);
    //
    //     //        var mapped = _mapper.Map<T>(category);
    //
    //     //        dtoList.Add(mapped);
    //     //    });
    //
    //     //    return await Task.FromResult(dtoList);
    //     //}
    //
    //     //private async Task<T?> MapCategory<T>(Category? category)
    //     //{
    //     //    if (category is null)
    //     //    {
    //     //        return await Task.FromResult(default(T));
    //     //    }
    //
    //     //    category = await category.WithImagesAsync(_imageService);
    //
    //     //    var mapped = _mapper.Map<T>(category);
    //
    //     //    return mapped;
    //     //}
    // }
}
