using System.Linq.Expressions;
using api.Models;
using AutoMapper;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Expression = System.Linq.Expressions.Expression;

namespace api.Helpers
{
    public static class PriceHelper
    {
        public static Expression<Func<Item, IEnumerable<ItemPrice>>> GetLatestPriceExpression()
        {
            return item => item.ItemPrices.OrderByDescending(e => e.Date).Take(1);
        }

        // public static IMappingExpression CreatePriceMap(MapperConfiguration configg)
        // {
        //     // var config = new MapperConfiguration(cfg => {
        //     //     cfg.CreateMap<Product, ProductDTO>()
        //     //         .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.ProductPrices.FirstOrDefault().Price));
        //     // });
        //     
        //     return 
        //
        // }
    }
}
