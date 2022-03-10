using api.CustomExceptions;
using api.Data;
using api.Dtos.CategoryControllerDtos;
using api.Helpers;
using api.Models;
using AutoMapper;
using System.Data;

namespace api.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;
        private readonly IConfiguration _config;

        public CategoryService(
            ICategoryRepository categoryRepository, 
            IMapper mapper, 
            IImageService imageService,
            IConfiguration config)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _imageService = imageService;
            _config = config;
        }

        public async Task<CreateCategoryResponse?> CreateAsync(CreateCategoryRequest dto)
        {
            // return null if name is not unique
            var isUnique = await IsNameUniqueAsync(dto.Name);
            if (!isUnique)
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
                var parent = await _categoryRepository
                    .GetByIdAsync((int)dto.ParentCategoryId);

                if (parent is null)
                {
                    throw new ObjectNotFoundException();
                }

                category.Parent = parent;
            }

            // add the category with unique name to db
            await _categoryRepository.AddAsync(category);

            var result = await MapCategory<CreateCategoryResponse>(category);

            return result;
        }

        public async Task<List<SearchCategoryWithItemsResponse>> FindCategoryAsync(string searchKey)
        {
            searchKey = searchKey.Trim();

            var categories = await _categoryRepository.FindCategoryWithItemsAsync(searchKey);

            var result = await MapCategories<SearchCategoryWithItemsResponse>(categories);

            return result;
        }

        public async Task<GetCategoryResponse?> GetCategoryByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            var result = await MapCategory<GetCategoryResponse>(category);

            return result;
        }

        public async Task<GetCategoryWithItemsResponse?> GetCategoryWithItemsByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdWithItemsAsync(id);

            var result = await MapCategory<GetCategoryWithItemsResponse>(category);

            return result;
        }

        public async Task<List<GetAllCategoriesResponse>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();

            var result = await MapCategories<GetAllCategoriesResponse>(categories);

            return result;
        }

        public async Task<List<GetAllCategoriesWithItemsResponse>> GetAllCategoriesWithItemsAsync()
        {
            var categories = await _categoryRepository.GetAllWithItemsAsync();

            var result = await MapCategories<GetAllCategoriesWithItemsResponse>(categories);
            
            return result;
        }

        public async Task<UpdateCategoryResponse?> UpdateAsync(UpdateCategoryRequest dto)
        {
            // category photo update
            var category = await _categoryRepository.GetByIdWithItemsAsync(dto.Id);
            if(category is null)
            {
                throw new ObjectNotFoundException();
            }

            if(dto.Name != null)
            {
                var isUnique = await IsNameUniqueAsync(dto.Name);
                if (!isUnique)
                {
                    throw new DuplicateNameException();
                }
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
                category.ImageName = await _imageService.SaveImageAsync(dto.ImageToUpload) ??
                    defaultImageName;
            }

            await _categoryRepository.SaveChangesAsync();

            var result = await MapCategory<UpdateCategoryResponse>(category);

            return result;
        }

        public async Task DeleteByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdWithItemsAsync(id);
            if (category is null)
            {
                throw new ObjectNotFoundException();
            }
            if (category.Items.Count > 0)
            {
                throw new RelatedDataException();
            }

            // delete image file
            await _imageService.DeleteImageFileAsync(category.ImageName);

            // delete the category
            await _categoryRepository.DeleteAsync(category);
        }


        // helpers
        private async Task<bool> IsNameUniqueAsync(string name)
        {
            var existingCategory = await _categoryRepository.GetByNameAsync(name);
            if (existingCategory is null)
            {
                return true;
            }

            return false;
        }

        private async Task<List<T>> MapCategories<T>(List<Category>? categories)
        {
            var dtoList = new List<T>();

            if (categories != null)
            {
                categories.ForEach(async category =>
                {
                    // add item and user images
                    category = await category.WithImagesAsync(_imageService);

                    var mapped = _mapper.Map<T>(category);

                    dtoList.Add(mapped);
                });
            }

            return await Task.FromResult(dtoList);
        }

        private async Task<T?> MapCategory<T>(Category? category)
        {
            if (category is null)
            {
                return await Task.FromResult(default(T));
            }

            category = await category.WithImagesAsync(_imageService);

            var mapped = _mapper.Map<T>(category);

            return mapped;
        }
    }
}
