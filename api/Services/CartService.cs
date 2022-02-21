using api.Data;
using api.Dtos.CartControllerDtos;
using api.Models;
using AutoMapper;
using api.CustomExceptions;
using System.Security.Claims;
using api.Helpers;

namespace api.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IUserRepository _userRepository;
        private readonly IImageService _imageService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public CartService(
            ICartRepository cartRepository,
            IItemRepository itemRepository,
            IUserRepository userRepository,
            IImageService imageService,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _cartRepository = cartRepository;
            _itemRepository = itemRepository;
            _userRepository = userRepository;
            _imageService = imageService;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<AddToCartResponse> AddToCartAsync(int itemId)
        {
            // authorize user and check if item exists
            var user = await AuthorizeUser();
            var item = await GetItemAsync(itemId);
            var cart = await GetCartAsync(user.Id);

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
            cart.ModifiedDate = DateTimeOffset.Now;
            await _cartRepository.SaveChangesAsync();

            // append image source
            cart.CartItems.ForEach(cartItem =>
            {
                cartItem.Item!.Images.ForEach(async image =>
                    image.ImageSrc = await _imageService.GetImageSourceAsync(image.ImageName));
            });

            return _mapper.Map<AddToCartResponse>(cart);
        }

        public async Task<RemoveFromCartResponse?> RemoveFromCartAsync(int itemId)
        {
            // authorize user and check if item exists
            var user = await AuthorizeUser();
            var item = await GetItemAsync(itemId);
            var cart = await GetCartAsync(user.Id);

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
            cart.ModifiedDate = DateTimeOffset.Now;


            await _cartRepository.SaveChangesAsync();

            // append image source
            cart.CartItems.ForEach(cartItem =>
            {
                cartItem.Item!.Images.ForEach(async image =>
                    image.ImageSrc = await _imageService.GetImageSourceAsync(image.ImageName));
            });

            return _mapper.Map<RemoveFromCartResponse>(cart);
        }

        public async Task<GetCartWithItemsResponse> GetCurrentUserCartWithItemsAsync()
        {
            var user = await AuthorizeUser();

            var cart = await _cartRepository.GetCartWithItemsByIdAsync(user.Id);

            if (cart is null)
            {
                throw new ObjectNotFoundException();
            }

            // add images urls
            cart.CartItems.ForEach(cartItem =>
            {
                cartItem.Item!.Images.ForEach(async image =>
                    image.ImageSrc = await _imageService.GetImageSourceAsync(image.ImageName));
            });

            return _mapper.Map<GetCartWithItemsResponse>(cart);
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

        private async Task<Cart> GetCartAsync(int userId)
        {
            var cart = await _cartRepository.GetCartWithItemsByIdAsync(userId);

            if (cart == null)
            {
                cart = await _cartRepository.AddAsync(new Cart()
                {
                    UserId = userId,
                    TotalPrice = 0,
                    ModifiedDate = DateTimeOffset.Now,
                });
            }

            return cart;
        }
    }
}
