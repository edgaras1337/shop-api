using api.CustomExceptions;
using api.Dtos.WishlistControllerDtos;
using api.Helpers;
using api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using api.Repo;

namespace api.Services
{
    public class WishlistItemService : IWishlistItemService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IImageService _imageService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public WishlistItemService(
            UserManager<ApplicationUser> userManager,
            IImageService imageService,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _imageService = imageService;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        private IQueryable<WishlistItem> GetWishlistQuery(int userId)
        {
            return _unitOfWork.WishlistRepository.GetAllQuery()
                .Include(e => e.Item)
                .ThenInclude(e => e.Images)
                .Where(e => e.UserId == userId);
        }

        public async Task<List<AddWishlistItemResponse>> AddToWishlistAsync(int itemId)
        {
            var user = await AuthorizeUserAsync();
            var query = GetWishlistQuery(user.Id);
            var existingItem = await query
                .Where(e => e.ItemId == itemId)
                .SingleOrDefaultAsync();
            if (existingItem != null)
            {
                // Item is already in the wishlist.
                throw new DuplicateDataException();
            }

            // await _unitOfWork.WishlistRepository.AddAsync(new WishlistItem(user.Id, itemId));
            var newItem = new WishlistItem(user.Id, itemId);
            await _unitOfWork.WishlistRepository.AddAsync(newItem);
            await _unitOfWork.SaveChangesAsync();
            var userWishlist = await query.ToListAsync();
            
            await _imageService.LoadImagesAsync(userWishlist);
            return _mapper.Map<List<AddWishlistItemResponse>>(userWishlist);
        }

        public async Task<List<RemoveWishlistItemResponse>> RemoveWishlistItemAsync(int itemId)
        {
            var user = await AuthorizeUserAsync();
            var query = GetWishlistQuery(user.Id);
            var delete = await query
                .Where(e => e.ItemId == itemId)
                .SingleOrDefaultAsync();
            if (delete == null)
            {
                throw new DuplicateDataException();
            }

            _unitOfWork.WishlistRepository.Delete(delete);
            await _unitOfWork.SaveChangesAsync();

            var userWishlist = await query.ToListAsync();
            await _imageService.LoadImagesAsync(userWishlist);
            return _mapper.Map<List<RemoveWishlistItemResponse>>(userWishlist);
        }

        public async Task<List<GetCurrentUserWishlistResponse>> GetCurrentUserWishlistAsync()
        {
            var user = await AuthorizeUserAsync();
            var wishlist = await GetWishlistQuery(user.Id).ToListAsync();
            await _imageService.LoadImagesAsync(wishlist);
            return _mapper.Map<List<GetCurrentUserWishlistResponse>>(wishlist);
        }

        // helpers
        private async Task<ApplicationUser> AuthorizeUserAsync()
        {
            var name = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
            if (name == null)
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
    }
}
