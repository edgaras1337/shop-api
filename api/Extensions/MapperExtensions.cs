using api.Dtos;
using api.Dtos.WishlistControllerDtos;
using api.Models;
using AutoMapper;

namespace api.Extensions;

public static class MapperExtensions
{
    // public static IMappingExpression<Item, AddWishlistItemResponse.ItemDto> MapSinglePrice(this IMappingExpression<Item, AddWishlistItemResponse.ItemDto> config)
    // {
    //     return config
    //         .ForMember(dest =>
    //             dest.Price, opt => opt.MapFrom(src => src.ItemPrices.FirstOrDefault()!.PriceValue));
    // }
    
    public static IMappingExpression<Item, ISinglePriceEntity> MapSinglePrice(this IMappingExpression<Item, ISinglePriceEntity> config)
    {
        return config
            .ForMember(dest =>
                dest.Price, opt => opt.MapFrom(src => src.ItemPrices.FirstOrDefault()!.PriceValue));
    }
}