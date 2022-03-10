using api.Models;
using AutoMapper;

namespace api.Dtos.PurchaseControllerDtos
{
    public class GetAllPurchaseHistoryResponse
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTimeOffset PurchaseDate { get; set; }
        public bool IsDelivered { get; set; }

        public List<GetAllPurchaseHistoryResponse_PurchaseItem> PurchaseItems { get; set; } = new();
        public GetAllPurchaseHistoryResponse_DeliveryAddress? DeliveryAddress { get; set; }
    }

    public class GetAllPurchaseHistoryResponse_PurchaseItem
    {
        public int? ItemId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

    public class GetAllPurchaseHistoryResponse_DeliveryAddress
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

    public class GetAllPurchaseHistoryResponseProfiles : Profile
    {
        public GetAllPurchaseHistoryResponseProfiles()
        {
            CreateMap<Purchase, GetAllPurchaseHistoryResponse>();
            CreateMap<PurchaseItem, GetAllPurchaseHistoryResponse_PurchaseItem>();
            CreateMap<DeliveryAddress, GetAllPurchaseHistoryResponse_DeliveryAddress>();
        }
    }
}
