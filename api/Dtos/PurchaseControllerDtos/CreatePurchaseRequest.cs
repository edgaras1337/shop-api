using api.Models;
using AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace api.Dtos.PurchaseControllerDtos
{
    public class CreatePurchaseRequest
    {
        public CreatePurchaseRequest_DeliveryAddress? DeliveryAddress { get; set; }
        public List<CreatePurchaseRequest_PurchaseItem> PurchaseItems { get; set; } = new();
    }

    public class CreatePurchaseRequest_PurchaseItem
    {
        [Required]
        public int ItemId { get; set; }
        [Required]
        public int Quantity { get; set; }
    }

    public class CreatePurchaseRequest_DeliveryAddress
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Surname { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;
        [Required]
        public string Country { get; set; } = string.Empty;
        [Required]
        public string City { get; set; } = string.Empty;
        [Required]
        public string Address { get; set; } = string.Empty;
        [Required]
        public string ZipCode { get; set; } = string.Empty;
    }

    public class CreatePurchaseRequestProfiles : Profile
    {
        public CreatePurchaseRequestProfiles()
        {
            CreateMap<CreatePurchaseRequest_PurchaseItem, PurchaseItem>();
            CreateMap<Item, PurchaseItem>();
            CreateMap<ApplicationUser, DeliveryAddress>();
            CreateMap<CreatePurchaseRequest_DeliveryAddress, DeliveryAddress>();
        }
    }
}
