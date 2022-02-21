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
                return null;
            }

            var category = _mapper.Map<Category>(dto);
            category.CreatedDate = DateTimeOffset.Now;
            category.ModifiedDate = category.CreatedDate;

            category.ImageName = await _imageService.SaveImageAsync(dto.ImageFile) ??
                _config["ImagesConfiguration:DefaultCategoryImageName"];

            // add the category with unique name to db
            await _categoryRepository.AddAsync(category);

            // add the image source
            var response = _mapper.Map<CreateCategoryResponse>(category);
            response.ImageSource = await _imageService.GetImageSourceAsync(response.ImageName);

            // map the category as output dto
            return response;
        }

        public async Task<List<SearchCategoryWithItemsResponse>> FindCategoryAsync
            (SearchCategoryWithItemsRequest dto)
        {
            var categories = await _categoryRepository.FindCategoryWithItemsAsync(dto.SearchKey);

            var dtoList = new List<SearchCategoryWithItemsResponse>();
            categories.ForEach(category =>
            {
                category.Items.ForEach(item =>
                {
                    item.Images.ForEach(async itemImage =>
                    {
                        itemImage.ImageSrc = 
                            await _imageService.GetImageSourceAsync(itemImage.ImageName);
                    });
                });
                dtoList.Add(_mapper.Map<SearchCategoryWithItemsResponse>(category));
            });

            return dtoList;
        }

        public async Task<GetCategoryResponse?> GetCategoryByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdWithItemsAsync(id);

            if (category is null)
            {
                return null;
            }

            var dto = _mapper.Map<GetCategoryResponse>(category);
            dto.ImageSource = await _imageService.GetImageSourceAsync(category.ImageName);

            return dto;
        }

        public async Task<GetCategoryWithItemsResponse?> GetCategoryWithItemsByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdWithItemsAsync(id);

            if(category is null)
            {
                return null;
            }

            // add item images
            category.Items.ForEach(item =>
            {
                item.Images.ForEach(async itemImage =>
                {
                    itemImage.ImageSrc =
                        await _imageService.GetImageSourceAsync(itemImage.ImageName);
                });

                item.Comments.ForEach(async comment =>
                {
                    comment.User!.ImageSrc = await _imageService.GetImageSourceAsync(comment.User.ImageName);
                });
            });

            var dto = _mapper.Map<GetCategoryWithItemsResponse>(category);
            dto.ImageSource = await _imageService.GetImageSourceAsync(category.ImageName);

            return dto;
        }

        public async Task<List<GetAllCategoriesResponse>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();

            var dto = new List<GetAllCategoriesResponse>();

            if (categories is null)
            {
                // return empty dto list
                return dto;
            }

            categories.ForEach(async category =>
            {
                var mapped = _mapper.Map<GetAllCategoriesResponse>(category);
                mapped.ImageSource = await _imageService.GetImageSourceAsync(category.ImageName);

                dto.Add(mapped);
            });

            return dto;
        }

        public async Task<List<GetAllCategoriesWithItemsResponse>> GetAllCategoriesWithItemsAsync()
        {
            var categories = await _categoryRepository.GetAllWithItemsAsync();

            var dto = new List<GetAllCategoriesWithItemsResponse>();

            if (categories is null)
            {
                // return empty dto list
                return dto;
            }

            categories.ForEach(async category =>
            {
                // add item and user images
                category.Items.ForEach(item =>
                {
                    item.Images.ForEach(async itemImage =>
                    {
                        itemImage.ImageSrc =
                            await _imageService.GetImageSourceAsync(itemImage.ImageName);
                    });

                    item.Comments.ForEach(async comment =>
                    {
                        comment.User!.ImageSrc = await _imageService.GetImageSourceAsync(comment.User.ImageName);
                    });
                });

                var mapped = _mapper.Map<GetAllCategoriesWithItemsResponse>(category);
                mapped.ImageSource = await _imageService.GetImageSourceAsync(category.ImageName);

                dto.Add(mapped);
            });
            
            return dto;
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

            category.ModifiedDate = DateTimeOffset.Now;

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

            category.Items.ForEach(async item => await AppendItemImageSrc(item));
            var response = _mapper.Map<UpdateCategoryResponse>(category);
            response.ImageSource = await _imageService.GetImageSourceAsync(category.ImageName);

            return response;
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

        private async Task<Item> AppendItemImageSrc(Item item)
        {
            item.Images.ForEach(async image => 
                image.ImageSrc = await _imageService.GetImageSourceAsync(image.ImageName));

            return await Task.FromResult(item);
        }

        private async Task<List<T>> MapCategories<T>(List<Category> categories, List<T> dtoList)
        {
            categories.ForEach(async category =>
            {
                // add item images
                category.Items.ForEach(async item => await AppendItemImageSrc(item));

                // add to dto
                var dto = _mapper.Map<dynamic>(category);
                dto.ImageSource = await _imageService.GetImageSourceAsync(category.ImageName);

                dtoList.Add(dto);
            });

            return await Task.FromResult(dtoList);
        }
    }
}
