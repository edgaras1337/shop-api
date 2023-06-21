using System.Data;
using api.CustomExceptions;
using api.Dtos.CategoryControllerDtos;
using api.Helpers;
using api.Models;
using api.Repo;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class CategoryService : ICategoryService
{
    private readonly IMapper _mapper;
    private readonly IImageService _imageService;
    private readonly IConfiguration _config;
    private readonly IUnitOfWork _unitOfWork;

    public CategoryService(
        IMapper mapper, 
        IImageService imageService,
        IConfiguration config,
        IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _imageService = imageService;
        _config = config;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateCategoryResponse?> CreateAsync(CreateCategoryRequest dto)
    {
        // return null if name is not unique
        if (!await IsNameUniqueAsync(dto.Name))
        {
            throw new DuplicateDataException();
        }

        var category = _mapper.Map<Category>(dto);
        category.CreatedDate = DateTimeOffset.UtcNow;
        category.ModifiedDate = category.CreatedDate;

        category.ImageName = await _imageService.SaveImageAsync(dto.ImageFile) ??
            _config["ImagesConfiguration:DefaultCategoryImageName"];

        if (dto.ParentCategoryId != null)
        {
            var parent = await _unitOfWork.CategoryRepository.GetByIdAsync(dto.ParentCategoryId.Value) ?? throw new ObjectNotFoundException();
            category.ParentCategory = parent;
        }

        // add the category with unique name to db
        await _unitOfWork.CategoryRepository.AddAsync(category);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<CreateCategoryResponse>(category);
    }

    public async Task<GetCategoryResponse?> GetCategoryByIdAsync(GetCategoryParams getParams)
    {
        var categoriesQuery = _unitOfWork.CategoryRepository.GetAllQuery().AsNoTracking()
            .Where(e => e.Id == getParams.Id);

        Category? category;
        if (getParams.IncludeChildren)
        {
            category = await categoriesQuery
                .Include(e => e.ChildCategories)
                .SingleOrDefaultAsync();
            if (category == null)
            {
                return null;
            }

            if (category.ChildCategories != null)
            {
                await MapChildrenAsync(category.ChildCategories);
            }
        }
        else
        {
            category = await categoriesQuery.SingleAsync();
        }

        await _imageService.LoadImagesAsync(category);
        return _mapper.Map<GetCategoryResponse>(category);
    }

    public async Task<List<GetCategoriesResponse>?> GetCategoriesAsync(GetCategoriesParams getParams)
    {
        var categoriesQuery = _unitOfWork.CategoryRepository.GetAllQuery().AsNoTracking();

        if (getParams.IncludeItems)
        {
            categoriesQuery = categoriesQuery.Include(e => e.Items);
        }

        var list = await categoriesQuery.ToListAsync();
        foreach (var item in list.ToList().Where(item => item.ParentCategoryId != null))
        {
            list.Remove(item);
        }

        await _imageService.LoadImagesAsync(list);
        return _mapper.Map<List<GetCategoriesResponse>>(list);
    }

    public async Task<UpdateCategoryResponse?> UpdateAsync(UpdateCategoryRequest dto)
    {
        // category photo update
        var category = await _unitOfWork.CategoryRepository.GetAllQuery()
            .Include(e => e.Items)
            .SingleOrDefaultAsync(e => e.Id == dto.Id) ?? throw new ObjectNotFoundException();
        if (dto.Name != null && !await IsNameUniqueAsync(dto.Name))
        {
            throw new DuplicateNameException();
        }

        _mapper.Map(dto, category);

        category.ModifiedDate = DateTimeOffset.UtcNow;

        // update image
        var defaultImageName = _config["ImagesConfiguration:DefaultCategoryImageName"];
        if (dto.DeleteImage)
        {
            // delete image only when its not the default one
            // when deleted set the default image
            if (category.ImageName != defaultImageName)
            {
                await _imageService.DeleteImageFileAsync(category.ImageName);
                category.ImageName = defaultImageName;
            }
        }
        if (dto.ImageToUpload != null)
        {
            // save the selected image, if failed, set default image
            category.ImageName = await _imageService.SaveImageAsync(dto.ImageToUpload) ?? defaultImageName;
        }

        await _unitOfWork.SaveChangesAsync();

        await _imageService.LoadImagesAsync(category);
        return _mapper.Map<UpdateCategoryResponse>(category);
    }

    public async Task DeleteByIdAsync(int id)
    {
        var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id) ?? throw new ObjectNotFoundException();

        // Cannot delete if items still exist.
        if (category.Items.Count > 0)
        {
            throw new RelatedDataException();
        }

        // delete image file
        await _imageService.DeleteImageFileAsync(category.ImageName);

        // delete the category
        _unitOfWork.CategoryRepository.Delete(category);
        await _unitOfWork.SaveChangesAsync();
    }
    
    
    // helpers
    private async Task<bool> IsNameUniqueAsync(string name)
    {
        return (await _unitOfWork.CategoryRepository.GetAllQuery().AsNoTracking().SingleOrDefaultAsync(e => e.Name == name)) == null;
    }

    private async Task MapChildrenAsync(List<Category> categories)
    {
        foreach (var category in categories)
        {
            IQueryable<Category> query = _unitOfWork.CategoryRepository.GetAllQuery().AsNoTracking()
                .Where(e => e.ParentCategoryId == category.Id)
                .Include(e => e.ChildCategories);

            category.ChildCategories = await query.ToListAsync();

            if (category.ChildCategories?.Any() ?? false)
            {
                await MapChildrenAsync(category.ChildCategories);
            }
        }
    }
}
