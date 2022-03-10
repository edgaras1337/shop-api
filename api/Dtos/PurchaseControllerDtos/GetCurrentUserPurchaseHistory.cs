﻿using api.Models;
using AutoMapper;

namespace api.Dtos.PurchaseControllerDtos
{
    public class GetCurrentUserPurchaseHistory
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public decimal TotalPrice { get; set; }
        public DateTimeOffset PurchaseDate { get; set; }
        public bool IsDelivered { get; set; }

        public List<GetCurrentUserPurchaseHistoryResponse_PurchaseItem> PurchaseItems { get; set; } = new();
        public GetCurrentUserPurchaseHistoryResponse_DeliveryAddress? DeliveryAddress { get; set; }
    }

    public class GetCurrentUserPurchaseHistoryResponse_PurchaseItem
    {
        public int? ItemId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

    public class GetCurrentUserPurchaseHistoryResponse_DeliveryAddress
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

    public class GetCurrentUserPurchaseHistoryResponseProfiles : Profile
    {
        public GetCurrentUserPurchaseHistoryResponseProfiles()
        {
            CreateMap<Purchase, GetCurrentUserPurchaseHistory>();
            CreateMap<PurchaseItem, GetCurrentUserPurchaseHistoryResponse_PurchaseItem>();
            CreateMap<DeliveryAddress, GetCurrentUserPurchaseHistoryResponse_DeliveryAddress>();
        }
    }
}
