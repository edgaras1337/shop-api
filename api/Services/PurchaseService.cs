using api.CustomExceptions;
using api.Data;
using api.Dtos.PurchaseControllerDtos;
using api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IItemRepository _itemRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public PurchaseService(
            IPurchaseRepository purchaseRepository,
            IItemRepository itemRepository,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _purchaseRepository = purchaseRepository;
            _itemRepository = itemRepository;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<CreatePurchaseResponse> CreatePurchaseAsync(CreatePurchaseRequest request)
        {
            var user = await AuthorizeAsync();

            Purchase purchase;

            // purhcase if user isnt aunthorized
            if (user is null)
            {
                // add purchase to db
                var items = await GetItemsAsync(request.PurchaseItems);

                purchase = new Purchase();

                await _purchaseRepository.AddAsync(purchase);

                // add purchase items to purchase
                for (int i = 0; i < items.Count - 1; i++)
                {
                    purchase.PurchaseItems.Add(new PurchaseItem
                    {

                        ItemId = request.PurchaseItems[i].ItemId,
                        Name = items[i].Name,
                        Quantity = request.PurchaseItems[i].Quantity,
                        Price = Convert.ToDecimal(items[i].Price),
                        PurchaseId = purchase.Id
                    });

                    purchase.TotalPrice += request.PurchaseItems[i].Quantity *
                        Convert.ToDecimal(items[i].Price);

                    items[i].Quantity -= request.PurchaseItems[i].Quantity;
                }

                // add delivery address to purchase
                purchase.DeliveryAddress = _mapper.Map<DeliveryAddress>(request.DeliveryAddress);
                purchase.DeliveryAddress.PurchaseId = purchase.Id;

                // save changes
                await _purchaseRepository.SaveChangesAsync();
            }
            else
            {
                purchase = new Purchase(user.Id);

                if (user.Cart is null || !user.Cart.CartItems.Any())
                {
                    throw new ObjectNotFoundException();
                }

                purchase.TotalPrice = user.Cart.TotalPrice;

                await _purchaseRepository.AddAsync(purchase);

                foreach (var cartItem in user.Cart.CartItems)
                {
                    purchase.PurchaseItems.Add(new PurchaseItem
                    {
                        ItemId = cartItem.ItemId,
                        Name = cartItem.Item!.Name,
                        Quantity = cartItem.Quantity,
                        Price = Convert.ToDecimal(cartItem.Item.Price),
                        PurchaseId = purchase.Id
                    });
                }

                if (request.DeliveryAddress != null)
                {
                    purchase.DeliveryAddress = _mapper.Map<DeliveryAddress>(request.DeliveryAddress);
                }
                else
                {
                    purchase.DeliveryAddress = _mapper.Map<DeliveryAddress>(user);
                }

                purchase.DeliveryAddress.PurchaseId = purchase.Id;

                user.Cart.CartItems.Clear();
                user.Cart.TotalPrice = 0;

                // save changes
                await _purchaseRepository.SaveChangesAsync();
            }

            var result = _mapper.Map<CreatePurchaseResponse>(purchase);

            return result;
        }

        public async Task<List<GetCurrentUserPurchaseHistory>> GetCurrentUserPurchaseHistoryAsync()
        {
            var user = await AuthorizeAsync();

            if (user is null)
            {
                throw new UnauthorizedException();
            }

            var purchases = await _purchaseRepository.GetByUserIdAsync(user.Id);

            if (purchases is null || purchases.Count == 0)
            {
                throw new ObjectNotFoundException();
            }

            var result = _mapper.Map<List<GetCurrentUserPurchaseHistory>>(purchases);

            return result;
        }

        public async Task<List<GetAllPurchaseHistoryResponse>> GetAllPurchaseHistoryAsync()
        {
            var purchases = await _purchaseRepository.GetAllAsync();

            if (purchases is null || purchases.Count == 0)
            {
                throw new ObjectNotFoundException();
            }

            var result = _mapper.Map<List<GetAllPurchaseHistoryResponse>>(purchases);

            return result;
        }

        public async Task<List<GetUserPurchaseHistory>> GetUserPurchaseHistoryAsync(int userId)
        {
            var purchases = await _purchaseRepository.GetAllAsync();

            if (purchases is null || purchases.Count == 0)
            {
                throw new ObjectNotFoundException();
            }

            var result = _mapper.Map<List<GetUserPurchaseHistory>>(purchases);

            return result;
        }


        // helpers
        private async Task<List<Item>> GetItemsAsync(List<CreatePurchaseRequest_PurchaseItem> purchaeItems) 
        {
            var items = new List<Item>();

            foreach (var purchaseItem in purchaeItems)
            {
                var item = await _itemRepository.GetByIdAsync(purchaseItem.ItemId);

                if (item is null)
                {
                    throw new ObjectNotFoundException();
                }

                items.Add(item);
            }

            if (!items.Any())
            {
                throw new ObjectNotFoundException();
            }

            return items;
        }

        private async Task<ApplicationUser?> AuthorizeAsync()
        {
            var name = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;

            if (name is null)
            {

                return null;
            }

            var user = await _userManager.Users
                .Include(e => e.Cart)
                .SingleOrDefaultAsync(e => e.UserName == name);

            if (user is null)
            {
                return null;
            }

            return user;
        }
    }
}
