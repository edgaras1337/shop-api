using api.Dtos.CartControllerDtos;
using api.Models;
using AutoMapper;
using api.CustomExceptions;
using api.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using api.Repo;
using System.Linq.Expressions;

namespace api.Services
{
    public class CartService : ICartService
    {
        private readonly IImageService _imageService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        private IUnitOfWork _unitOfWork;

        public CartService(
            IImageService imageService,
            IHttpContextAccessor httpContextAccessor,
            UserManager<ApplicationUser> userManager,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _imageService = imageService;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<AddToCartResponse> AddToCartAsync(int itemId)
        {
            // authorize user and check if item exists
            var user = await AuthorizeUserAsync();
            var cart = user.Cart!;

            // check if item already exists in cart
            var cartItem = cart.CartItems
                .SingleOrDefault(e => e.Item!.Id == itemId);

            if(cartItem is null)
            {
                // create new cart item if such item doesnt exist
                cartItem = new CartItem()
                {
                    Quantity = 1,
                    ItemId = itemId,
                    CartId = user.Id,
                };

                // add item to cart and save the changes
                cart.CartItems.Add(cartItem);
            }
            else
            {
                // increase quantity if cart item already exists
                cartItem.Quantity++;
            }

            var price = await _unitOfWork.ItemPriceRepository
                .GetAllQuery()
                .OrderByDescending(p => p.Date)
                .FirstAsync();

            // update price
            cart.TotalPrice += price.PriceValue;
            cart.ModifiedDate = DateTimeOffset.UtcNow;

            // save changes in db context
            await _unitOfWork.SaveChangesAsync();
            await _imageService.LoadImagesAsync(cart);
            return _mapper.Map<AddToCartResponse>(cart);
        }

        public async Task<RemoveFromCartResponse?> RemoveFromCartAsync(int itemId)
        {
            // authorize user and check if item exists
            var user = await AuthorizeUserAsync();
            var cart = user.Cart!;

            // check if item exists in cart
            var existingCartItem = cart.CartItems
                .SingleOrDefault(e => e.ItemId == itemId);

            if (existingCartItem is null)
            {
                throw new ObjectNotFoundException();
            }

            existingCartItem.Quantity--;

            if (existingCartItem.Quantity == 0)
            {
                cart.CartItems.Remove(existingCartItem);
            }

            var price = await _unitOfWork.ItemPriceRepository
                .GetAllQuery()
                .OrderByDescending(p => p.Date)
                .FirstAsync();

            // update price
            cart.TotalPrice -= price.PriceValue;
            cart.ModifiedDate = DateTimeOffset.UtcNow;

            await _unitOfWork.SaveChangesAsync();
            await _imageService.LoadImagesAsync(cart);
            return _mapper.Map<RemoveFromCartResponse>(cart);
        }

        public async Task<GetCartResponse> GetCurrentUserCart()
        {
            var user = await AuthorizeUserAsync();
            var cart = await _unitOfWork.CartRepository
                .GetAllQuery()
                .Include(e => e.CartItems)
                .ThenInclude(e => e.Item)
                .ThenInclude(e => e!.Images)
                .Include(e => e.CartItems)
                .ThenInclude(e => e.Item)
                .ThenInclude(e => e!.Category)
                .SingleOrDefaultAsync(e => e.UserId == user.Id);

            if (cart is null)
            {
                throw new ObjectNotFoundException();
            }

            await _imageService.LoadImagesAsync(cart);
            return _mapper.Map<GetCartResponse>(cart);
        }

        public async Task RecalcCartPrice(Expression<Func<Cart, bool>> cartFilter)
        {
            var carts = await _unitOfWork.CartRepository
                .GetAllQuery()
                .Include(e => e.CartItems)
                .ThenInclude(e => e.Item)
                .ThenInclude(e => e!.ItemPrices.OrderByDescending(e => e.Date).Take(1))
                .Where(cartFilter)
                .ToListAsync();

            foreach (var cart in carts)
            {
                cart.TotalPrice = 0;
                foreach (var cartItem in cart.CartItems)
                {
                    // TODO: Check for currency mismatch.
                    cart.TotalPrice += cartItem.Item!.ItemPrices[0].PriceValue * cartItem.Quantity;
                }
            }

            await _unitOfWork.SaveChangesAsync();
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
                .Include(e => e.Cart)
                .Include(e => e.WishlistItems)
                .SingleOrDefaultAsync(e => e.UserName == name);

            if (user == null)
            {
                throw new UnauthorizedException();
            }

            /*if (user.Cart is null)
            {
                user.Cart = new Cart(user.Id);
            }*/

            return user;
        }
    }
}
