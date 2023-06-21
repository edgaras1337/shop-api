using api.Dtos;
using api.Extensions;
using api.Models;
using AutoMapper;

namespace api.Helpers;

public class SinglePriceProfile : Profile
{
    public SinglePriceProfile()
    {
        CreateMap<Item, ISinglePriceEntity>().MapSinglePrice();
    }
}