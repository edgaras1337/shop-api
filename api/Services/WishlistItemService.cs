using api.CustomExceptions;
using api.Data;
using api.Dtos.WishlistControllerDtos;
using api.Helpers;
using api.Models;
using AutoMapper;
using System.Security.Claims;

namespace api.Services
{
    public class WishlistItemService : IWishlistItemService
    {
        private readonly IWishlistItemRepository _wishlistItemRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IUserRepository _userRepository;
        private readonly IImageService _imageService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public WishlistItemService(
            IWishlistItemRepository wishlistItemRepository,
            IItemRepository itemRepository,
            IUserRepository userRepository,
            IImageService imageService,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _wishlistItemRepository = wishlistItemRepository;
            _itemRepository = itemRepository;
            _userRepository = userRepository;
            _imageService = imageService;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<List<AddWishlistItemResponse>> AddToWishlistAsync(int itemId)
        {
            var currentUser = await AuthorizeUser();
            var item = await GetItemAsync(itemId);

            // get all wishlist items or create a new list
            var wishlist = await _wishlistItemRepository.GetWishlistByUserId(currentUser.Id);
            if(wishlist is null)
            {
                wishlist = new List<WishlistItem>();
            }
            var existingWishlistItem = await _wishlistItemRepository
                .GetCartItemByUserAndItemIdAsync(currentUser.Id, item.Id);

            if(existingWishlistItem is null)
            {
                // add to wishlist for the dto and add to the db
                wishlist.Add(await _wishlistItemRepository.AddAsync(new WishlistItem()
                {
                    ItemId = item.Id,
                    UserId = currentUser.Id,
                }));
            }
            else
            {
                // exception when trying to add existing item to wihslist
                throw new DuplicateDataException();
            }

            var dto = new List<AddWishlistItemResponse>();
            return wishlist is null ? dto : await AppendImageSource(wishlist, dto);
        }

        public async Task<List<RemoveWishlistItemResponse>> RemoveWishlistItemAsync(int itemId)
        {
            var currentUser = await AuthorizeUser();
            var wishlist = await _wishlistItemRepository.GetWishlistByUserId(currentUser.Id);

            var itemToDelete = wishlist?
                .SingleOrDefault(e => e.ItemId == itemId);

            if (itemToDelete is null)
            {
                throw new ObjectNotFoundException();
            }
            // delete from the list which will be converted to dto
            wishlist?.Remove(itemToDelete);

            // delete from db
            await _wishlistItemRepository.DeleteAsync(itemToDelete);

            var dto = new List<RemoveWishlistItemResponse>();
            return wishlist is null ? dto : await AppendImageSource(wishlist, dto);
        }

        public async Task<List<GetCurrentUserWishlistResponse>> GetCurrentUserWishlistAsync()
        {
            var currentUser = await AuthorizeUser();
            var wishlist = await _wishlistItemRepository.GetWishlistByUserId(currentUser.Id);

            var dto = new List<GetCurrentUserWishlistResponse>();

            return wishlist is null ? dto : await AppendImageSource(wishlist, dto);
        }



        // helpers
        private async Task<User> AuthorizeUser()
        {
            var id = _httpContextAccessor.HttpContext?
                .User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (id == null)
            {
                throw new UnauthorizedAccessException();
            }
            var user = await _userRepository.GetByIdAsync(int.Parse(id));
            if (user == null)
            {
                throw new UnauthorizedAccessException();
            }
            return user;
        }

        private async Task<Item> GetItemAsync(int itemId)
        {
            var item = await _itemRepository.GetByIdAsync(itemId);
            if (item is null)
            {
                throw new ObjectNotFoundException();
            }
            return item;
        }

        private async Task<List<T>> AppendImageSource<T>(List<WishlistItem> wishlist, List<T> dto)
        {
            wishlist.ForEach(wishlistItem =>
            {
                wishlistItem.Item!.Images.ForEach(async image =>
                    image.ImageSrc = await _imageService.GetImageSourceAsync(image.ImageName));

                dto.Add(_mapper.Map<T>(wishlistItem));
            });

            return await Task.FromResult(dto);
        }
    }
}
