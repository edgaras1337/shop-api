using api.Data;
using api.Dtos.CartControllerDtos;
using api.Models;
using AutoMapper;
using api.CustomExceptions;
using System.Security.Claims;
using api.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IImageService _imageService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public CartService(
            ICartRepository cartRepository,
            IItemRepository itemRepository,
            IImageService imageService,
            IHttpContextAccessor httpContextAccessor,
            UserManager<ApplicationUser> userManager,
            IMapper mapper)
        {
            _cartRepository = cartRepository;
            _itemRepository = itemRepository;
            _imageService = imageService;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<AddToCartResponse> AddToCartAsync(int itemId)
        {
            // authorize user and check if item exists
            var user = await AuthorizeUserAsync();
            var item = await GetItemAsync(itemId);

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

            // update price
            cart.TotalPrice += (decimal)item.Price!;
            cart.ModifiedDate = DateTimeOffset.UtcNow;

            // save changes in db context
            await _cartRepository.SaveChangesAsync();

            var result = await MapCart<AddToCartResponse>(cart);

            return result;
        }

        public async Task<RemoveFromCartResponse?> RemoveFromCartAsync(int itemId)
        {
            // authorize user and check if item exists
            var user = await AuthorizeUserAsync();
            var item = await GetItemAsync(itemId);

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

            // update price
            cart.TotalPrice -= (decimal)item.Price!;
            cart.ModifiedDate = DateTimeOffset.UtcNow;

            await _cartRepository.SaveChangesAsync();

            var result = await MapCart<RemoveFromCartResponse>(cart);

            return result;
        }

        public async Task<GetCartResponse> GetCurrentUserCart()
        {
            var user = await AuthorizeUserAsync();

            var cart = await _cartRepository.GetCartWithItemsByIdAsync(user.Id);

            if (cart is null)
            {
                throw new ObjectNotFoundException();
            }

            var result = await MapCart<GetCartResponse>(cart);

            return result;
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

        private async Task<Item> GetItemAsync(int itemId)
        {
            var item = await _itemRepository.GetByIdAsync(itemId);
            if (item is null)
            {
                throw new ObjectNotFoundException();
            }
            return item;
        }

        private async Task<Cart> GetCartAsync(string userId)
        {
            var cart = await _cartRepository.GetCartWithItemsByIdAsync(userId);

            if (cart == null)
            {
                cart = await _cartRepository.AddAsync(new Cart(userId));
            }

            return cart;
        }

        private async Task<T> MapCart<T>(Cart cart)
        {
            cart = await cart.WithImages(_imageService);

            var mapped = _mapper.Map<T>(cart);

            return mapped;
        } 
    }
}
