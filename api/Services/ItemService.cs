using api.CustomExceptions;
using api.Dtos.ItemControllerDtos;
using api.Helpers;
using api.Models;
using api.Repo;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace api.Services
{
    // TODO: Check for currency mismatch.
    public class ItemService : IItemService
    {
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;
        private readonly IConfiguration _config;
        private readonly ICartService _cartService;
        private readonly IUnitOfWork _unitOfWork;

        public ItemService(
            IMapper mapper, 
            IImageService imageService,
            IConfiguration config,
            IUnitOfWork unitOfWork,
            ICartService cartService)
        {
            _mapper = mapper;
            _imageService = imageService;
            _config = config;
            _unitOfWork = unitOfWork;
            _cartService = cartService;
        }

        private GenericRepository<Item> ItemRepository => _unitOfWork.ItemRepository;
        private GenericRepository<Category> CategoryRepository => _unitOfWork.CategoryRepository;

        public async Task<CreateItemResponse?> CreateItemAsync(CreateItemRequest request)
        {
            // category check
            var category = await CategoryRepository.GetByIdAsync(request.CategoryId);
            if (category is null)
            {
                return null;
            }

            var itemModel = _mapper.Map<Item>(request);

            // add date
            itemModel.CreatedDate = DateTimeOffset.UtcNow;
            itemModel.ModifiedDate = itemModel.CreatedDate;

            // add item
            await _unitOfWork.ItemRepository.AddAsync(itemModel);

            // set category for dto
            itemModel.Category = category;

            await UploadImages(itemModel, request.ItemImageFiles);

            await _unitOfWork.SaveChangesAsync();

            await _imageService.LoadImagesAsync(itemModel);
            return _mapper.Map<CreateItemResponse>(itemModel);
        }

        public async Task<GetItemResponse?> GetItemByIdAsync(GetItemParams getParams)
        {
            var query = ItemRepository.GetAllQuery().AsNoTracking()
                .Where(e => e.Id == getParams.Id);

            if (getParams.IncludeReviews)
            {
                query = query.Include(e => e.Reviews.OrderByDescending(e => e.ModifiedDate))
                    .ThenInclude(e => e.User)
                    .ThenInclude(e => e!.UserRoles)
                    .ThenInclude(e => e.Role);
            }

            if (getParams.IncludeImages)
            {
                query = query.Include(e => e.Images);
            }

            var item = await query.SingleOrDefaultAsync();
            if (item == null)
            {
                return null;
            }

            await _imageService.LoadImagesAsync(item);
            return _mapper.Map<GetItemResponse>(item);
        }

        public async Task<GetItemsResponse> GetItems(GetItemsParams getParams)
        {
            IQueryable<Item> query = ItemRepository.GetAllQuery().AsNoTracking()
                .Include(e => e.Category)
                .Include(PriceHelper.GetLatestPriceExpression());
                // .Include(item => item.ItemPrices.OrderByDescending(e => e.Date).Take(1));

            if (getParams.IncludeReviews)
            {
                query = query.Include(e => e.Reviews.OrderByDescending(e => e.ModifiedDate))
                    .ThenInclude(e => e.User)
                    .ThenInclude(e => e!.UserRoles)
                    .ThenInclude(e => e.Role);
            }

            if (getParams.IncludeImages)
            {
                query = query.Include(e => e.Images);
            }

            if (!string.IsNullOrEmpty(getParams.SearchKey))
            {
                query = query.Where(e => e.Name.Contains(getParams.SearchKey));
            }

            if (getParams.IsFeatured)
            {
                query = query.Where(e => e.IsFeatured);
            }

            // // Apply dynamic ordering if necessary
            // if (!string.IsNullOrEmpty(getParams.OrderBy))
            // {
            //     query = query.OrderBy(getParams.OrderBy);
            // }

            // Apply pagination if necessary
            if (getParams.Offset.HasValue)
            {
                query = query.Skip(getParams.Offset.Value);
            }

            if (getParams.Count.HasValue)
            {
                query = query.Take(getParams.Count.Value);
            }

            // Return the filtered, ordered, and paginated items
            var itemList = await query.ToListAsync();
            if (getParams.IncludeImages)
            {
                await _imageService.LoadImagesAsync(itemList);
            }
            var dtoItems = _mapper.Map<IEnumerable<GetItemsResponse.ItemDto>>(itemList);
            return new GetItemsResponse(dtoItems);
        }

        public async Task<UpdateItemResponse> UpdateItemAsync(UpdateItemRequest request)
        {
            // check for existing item
            var item = await ItemRepository.GetByIdAsync(request.Id);
            if (item is null)
            {
                throw new ObjectNotFoundException();
            }
            // map dto to item model and update
            _mapper.Map(request, item);
            item.ModifiedDate = DateTimeOffset.UtcNow;
            await _unitOfWork.SaveChangesAsync();

            await _cartService.RecalcCartPrice(e => e.CartItems.FirstOrDefault(cartItem => cartItem.ItemId == item.Id) != null);

            if (item.Images.Count < request.ImageIdsToDelete.Count)
            {
                throw new InvalidOperationException();
            }
            await DeleteImages(item, request.ImageIdsToDelete);
            await UploadImages(item, request.ImageFilesToUpload);

            // save changes (updated images)
            await _unitOfWork.SaveChangesAsync();

            await _imageService.LoadImagesAsync(item);
            return _mapper.Map<UpdateItemResponse>(item);
        }

        public async Task DeleteItemAsync(int id)
        {
            var item = await ItemRepository.GetByIdAsync(id);
            if (item is null)
            {
                throw new ObjectNotFoundException();
            }

            await _cartService.RecalcCartPrice(e => e.CartItems.FirstOrDefault(cartItem => cartItem.ItemId == item.Id) != null);

            foreach (var image in item.Images.Where(image => image.ImageName != _config["ImagesConfiguration:DefaultItemImageName"]))
            {
                await _imageService.DeleteImageFileAsync(image.ImageName);
            }

            ItemRepository.Delete(item);
            await _unitOfWork.SaveChangesAsync();
        }

        private async Task UploadImages(Item itemModel, List<IFormFile> imagesToUpload)
        {
            // add the images
            if (!imagesToUpload.Any())
            {
                itemModel.Images.Add(new ItemImage()
                {
                    ImageName = _config["ImagesConfiguration:DefaultItemImageName"],
                    ItemId = itemModel.Id
                });
            }
            else
            {
                foreach (var imageFile in imagesToUpload)
                {
                    var img = await _imageService.SaveImageAsync(imageFile);
                    if (img is null) continue;

                    itemModel.Images.Add(new ItemImage()
                    {
                        ImageName = img,
                        ItemId = itemModel.Id,
                    });
                }
            }
        }

        private async Task DeleteImages(Item itemModel, List<int> imageIdsToDelete)
        {
            var defaultImageName = _config["ImagesConfiguration:DefaultItemImageName"];
            foreach (var imageId in imageIdsToDelete)
            {
                var image = itemModel.Images.FirstOrDefault(e => e.Id == imageId);

                if (image is null)
                {
                    throw new ObjectNotFoundException();
                }

                // skip the removal of default image
                if (image.ImageName == defaultImageName) continue;

                // remove image from the list
                itemModel.Images.Remove(image);

                // delete physical file
                await _imageService.DeleteImageFileAsync(image.ImageName);
            }
        }
    }
}
