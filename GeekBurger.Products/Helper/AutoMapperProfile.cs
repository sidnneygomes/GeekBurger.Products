using AutoMapper;
using GeekBurger.Products.Models;
using GeekBurger.Service.Contract;

namespace GeekBurger.Products.Helper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Product, ProductToGet>();
            CreateMap<Item, ItemToGet>();
        }


    }
}
