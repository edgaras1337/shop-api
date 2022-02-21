using api.CustomExceptions;
using api.Data;
using api.Dtos.ItemControllerDtos;
using api.Helpers;
using api.Models;
using AutoMapper;

namespace api.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;
        private readonly IConfiguration _config;

        public ItemService(
            IItemRepository itemRepository, 
            ICategoryRepository categoryRepository,
            ICartRepository cartRepository,
            IMapper mapper, 
            IImageService imageService,
            IConfiguration config)
        {
            _itemRepository = itemRepository;
            _categoryRepository = categoryRepository;
            _cartRepository = cartRepository;
            _mapper = mapper;
            _imageService = imageService;
            _config = config;
        }

        public async Task<CreateItemResponse?> CreateItemAsync(CreateItemRequest request)
        {
            // category check
            var category = await _categoryRepository.GetByIdAsync(request.CategoryId);
            if (category is null)
            {
                return null;
            }

            var itemModel = _mapper.Map<Item>(request);

            // add date
            itemModel.CreatedDate = DateTimeOffset.Now;
            itemModel.ModifiedDate = itemModel.CreatedDate;

            // add item
            await _itemRepository.AddAsync(itemModel);

            // set category for dto
            itemModel.Category = category;

            // add the images
            if (request.ItemImageFiles.Count == 0)
            {
                itemModel.Images.Add(new ItemImage()
                {
                    ImageName = _config["ImagesConfiguration:DefaultItemImageName"],
                    ItemId = itemModel.Id
                });
            }
            else
            {
                foreach (var imageFile in request.ItemImageFiles)
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

            // save changes (added images)
            await _itemRepository.SaveChangesAsync();

            return _mapper.Map<CreateItemResponse>((await AppendImageSrc(itemModel)));
        }

        public async Task<List<SearchItemResponse>> FindItemAsync(SearchItemRequest dto)
        {
            var items = await _itemRepository.FindAsync(dto.SearchKey);

            var dtoList = new List<SearchItemResponse>();
            items.ForEach(async item =>
            {
                item = await AppendImageSrc(item);
                dtoList.Add(_mapper.Map<SearchItemResponse>(item));
            });

            return dtoList;
        }

        public async Task<GetItemResponse?> GetItemByIdAsync(int id)
        {
            var item = await _itemRepository.GetByIdAsync(id);

            if (item is null)
            {
                return null;
            }

            // append user image
            item.Comments.ForEach(async comment =>
                comment.User!.ImageSrc = await _imageService.GetImageSourceAsync(comment.User.ImageName));

            item = await AppendImageSrc(item);

            return _mapper.Map<GetItemResponse>(item);
        }

        public async Task<List<GetAllItemsResponse>> GetAllItemsAsync()
        {
            var items = await _itemRepository.GetAllAsync();

            var itemsDto = new List<GetAllItemsResponse>();
            foreach (var item in items)
            {
                item.Comments.ForEach(async comment =>
                    comment.User!.ImageSrc = await _imageService.GetImageSourceAsync(comment.User.ImageName));

                var itemWithImages = await AppendImageSrc(item);

                itemsDto.Add(_mapper.Map<GetAllItemsResponse>(itemWithImages));
            }

            return itemsDto;
        }

        public async Task<UpdateItemResponse> UpdateItemAsync(UpdateItemRequest dto)
        {
            // check for existing item
            var item = await _itemRepository.GetByIdAsync(dto.Id);
            if (item is null)
            {
                throw new ObjectNotFoundException();
            }

            // map dto to item model and update
            _mapper.Map(dto, item);

            // update carts, where item was updated
            var cartsWithUpdatedItem = await _cartRepository.GetCartsByItemId(item.Id);
            if (cartsWithUpdatedItem != null)
            {
                cartsWithUpdatedItem.ForEach(cart =>
                {
                    cart.TotalPrice = 0;
                    cart.CartItems.ForEach(cartItem =>
                        cart.TotalPrice += (decimal)cartItem.Item!.Price! * cartItem.Quantity);
                });

                await _cartRepository.SaveChangesAsync();
            }

            // set modified date
            item.ModifiedDate = DateTimeOffset.Now;

            // save changes (updated properties)
            await _itemRepository.SaveChangesAsync();

            var defaultImageName = _config["ImagesConfiguration:DefaultItemImageName"];

            // delete images
            if (item.Images.Count < dto.ImageIdsToDelete.Count)
            {
                throw new InvalidOperationException();
            }

            foreach (var imageId in dto.ImageIdsToDelete)
            {
                var image = item.Images.FirstOrDefault(e => e.Id == imageId);

                if (image is null)
                {
                    throw new ObjectNotFoundException();
                }

                // skip the removal of default image
                if (image.ImageName == defaultImageName) continue;

                // remove image from the list
                item.Images.Remove(image);

                // delete physical file
                await _imageService.DeleteImageFileAsync(image.ImageName);
            }


            // upload images
            foreach (var imageFile in dto.ImageFilesToUpload)
            {
                // create physical image
                var savedImageName = await _imageService.SaveImageAsync(imageFile);

                if (savedImageName is null) continue;

                var newImage = new ItemImage()
                {
                    ImageName = savedImageName,
                    ItemId = item.Id
                };

                // add to list which will be added to db
                item.Images.Add(newImage);
            }

            if (item.Images.Count == 0)
            {
                // add the default image, if item has no images
                item.Images.Add(new ItemImage()
                {
                    ImageName = defaultImageName,
                    ItemId = item.Id
                });
            }
            else if (item.Images.Count > 1)
            {
                // if item has more than 1 image, delete the default one
                var defaultImage = item.Images
                    .SingleOrDefault(img => img.ImageName == defaultImageName);

                if (defaultImage != null)
                {
                    item.Images.Remove(defaultImage);
                }
            }

            // save changes (updated images)
            await _itemRepository.SaveChangesAsync();

            var response = _mapper.Map<UpdateItemResponse>(await AppendImageSrc(item));

            return response;
        }

        public async Task DeleteItemAsync(int id)
        {
            var item = await _itemRepository.GetByIdAsync(id);
            if (item is null)
            {
                throw new ObjectNotFoundException();
            }

            // update price of carts, which contain the item
            var cartsWithSelectedItem = await _cartRepository.GetCartsByItemId(item.Id);
            if (cartsWithSelectedItem != null)
            {
                cartsWithSelectedItem.ForEach(cart =>
                {
                    var itemToDelete = cart.CartItems
                        .SingleOrDefault(cartItem => cartItem.ItemId == item.Id);

                    cart.TotalPrice -= (decimal)itemToDelete!.Item!.Price! * itemToDelete.Quantity;
                });

                await _cartRepository.SaveChangesAsync();
            }

            foreach (var image in item.Images)
            {
                if(image.ImageName == _config["ImagesConfiguration:DefaultItemImageName"]) continue;

                await _imageService.DeleteImageFileAsync(image.ImageName);
            }

            await _itemRepository.DeleteAsync(item);
        }


        // helpers
        private async Task<Item> AppendImageSrc(Item item)
        {
            foreach(var image in item.Images)
            {
                image.ImageSrc = await _imageService.GetImageSourceAsync(image.ImageName);
            }

            return item;
        }
    }
}
