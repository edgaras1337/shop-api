using api.CustomExceptions;
using api.Data;
using api.Dtos.WishlistControllerDtos;
using api.Helpers;
using api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace api.Services
{
    public class WishlistItemService : IWishlistItemService
    {
        private readonly IWishlistItemRepository _wishlistItemRepository;
        private readonly IItemRepository _itemRepository;
        //private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IImageService _imageService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public WishlistItemService(
            IWishlistItemRepository wishlistItemRepository,
            IItemRepository itemRepository,
            //IUserRepository userRepository,
            UserManager<ApplicationUser> userManager,
            IImageService imageService,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _wishlistItemRepository = wishlistItemRepository;
            _itemRepository = itemRepository;
            _userManager = userManager;
            //_userRepository = userRepository;
            _imageService = imageService;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<List<AddWishlistItemResponse>> AddToWishlistAsync(int itemId)
        {
            var user = await AuthorizeUserAsync();
            var item = await GetItemAsync(itemId);

            // get all wishlist items or create a new list
            var wishlist = await _wishlistItemRepository.GetWishlistByUserId(user.Id);
            if(wishlist is null)
            {
                wishlist = new List<WishlistItem>();
            }
            var existingWishlistItem = await _wishlistItemRepository
                .GetCartItemByUserAndItemIdAsync(user.Id, item.Id);

            if(existingWishlistItem is null)
            {
                // add to wishlist for the dto and add to the db
                wishlist.Add(await _wishlistItemRepository.AddAsync(new WishlistItem()
                {
                    ItemId = item.Id,
                    UserId = user.Id,
                    AddedDate = DateTimeOffset.UtcNow,
                }));
            }
            else
            {
                // exception when trying to add existing item to wihslist
                throw new DuplicateDataException();
            }

            var dto = new List<AddWishlistItemResponse>();

            wishlist = await wishlist.WithImages(_imageService);

            foreach (var wishlistItem in wishlist)
            {
                var dtoItem = _mapper.Map<AddWishlistItemResponse>(wishlistItem);
                dto.Add(dtoItem);
            }

            return dto;

            //return wishlist is null ? dto : await AppendImageSource(wishlist, dto);
        }

        public async Task<List<RemoveWishlistItemResponse>> RemoveWishlistItemAsync(int itemId)
        {
            var currentUser = await AuthorizeUserAsync();
            var wishlist = await _wishlistItemRepository.GetWishlistByUserId(currentUser.Id);

            var dto = new List<RemoveWishlistItemResponse>();
            if (wishlist is null)
            {
                return dto;
            }

            var itemToDelete = wishlist
                .SingleOrDefault(e => e.ItemId == itemId);

            if (itemToDelete is null)
            {
                throw new ObjectNotFoundException();
            }
            // delete from the list which will be converted to dto
            wishlist.Remove(itemToDelete);

            // delete from db
            await _wishlistItemRepository.DeleteAsync(itemToDelete);

            wishlist = await wishlist.WithImages(_imageService);

            foreach (var wishlistItem in wishlist)
            {
                var dtoItem = _mapper.Map<RemoveWishlistItemResponse>(wishlistItem);
                dto.Add(dtoItem);
            }

            return dto;
        }

        public async Task<List<GetCurrentUserWishlistResponse>> GetCurrentUserWishlistAsync()
        {
            var currentUser = await AuthorizeUserAsync();
            var wishlist = await _wishlistItemRepository.GetWishlistByUserId(currentUser.Id);

            var dto = new List<GetCurrentUserWishlistResponse>();
            if (wishlist is null)
            {
                return dto;
            }

            wishlist = await wishlist.WithImages(_imageService);

            foreach (var wishlistItem in wishlist)
            {
                var dtoItem = _mapper.Map<GetCurrentUserWishlistResponse>(wishlistItem);
                dto.Add(dtoItem);
            }

            return dto;
        }



        // helpers
        private async Task<ApplicationUser> AuthorizeUserAsync()
        {
            var name = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;

            if (name is null)
            {
                throw new UnauthorizedException();
            }

            var user = await _userManager.Users
                .SingleOrDefaultAsync(e => e.UserName == name);

            if (user is null)
            {
                throw new UnauthorizedException();
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
                    image.ImageSource = await _imageService.GetImageSourceAsync(image.ImageName));

                dto.Add(_mapper.Map<T>(wishlistItem));
            });

            return await Task.FromResult(dto);
        }
    }
}
