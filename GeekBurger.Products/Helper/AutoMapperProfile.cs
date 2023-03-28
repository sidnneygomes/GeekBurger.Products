using AutoMapper;
using GeekBurger.Products.Contract;
using GeekBurger.Products.Models;
using GeekBurger.Service.Contract;

namespace GeekBurger.Products.Helper {
    public class AutoMapperProfile : Profile {
        public AutoMapperProfile() {
            CreateMap<Contract.Product, ProductToGet>();
            CreateMap<Contract.Item, ItemToGet>();
            CreateMap<ProductToUpsert, Contract.Product>()
                .AfterMap<MatchStoreFromRepository>();
            CreateMap<Contract.ItemToUpsert, Contract.Item>()
                .AfterMap<MatchItemsFromRepository>();

        }
    }
}
