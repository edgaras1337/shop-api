using api.Models;
using AutoMapper;

namespace api.Dtos.PurchaseControllerDtos
{
    public class GetUserPurchaseHistory
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTimeOffset PurchaseDate { get; set; }
        public bool IsDelivered { get; set; }

        public List<GetUserPurchaseHistoryResponse_PurchaseItem> PurchaseItems { get; set; } = new();
        public GetUserPurchaseHistoryResponse_DeliveryAddress? DeliveryAddress { get; set; }
    }

    public class GetUserPurchaseHistoryResponse_PurchaseItem
    {
        public int? ItemId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

    public class GetUserPurchaseHistoryResponse_DeliveryAddress
    {
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
    }

    public class GetUserPurchaseHistoryResponseProfiles : Profile
    {
        public GetUserPurchaseHistoryResponseProfiles()
        {
            CreateMap<Purchase, GetUserPurchaseHistory>();
            CreateMap<PurchaseItem, GetUserPurchaseHistoryResponse_PurchaseItem>();
            CreateMap<DeliveryAddress, GetUserPurchaseHistoryResponse_DeliveryAddress>();
        }
    }
}
