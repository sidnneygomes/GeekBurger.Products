using GeekBurger.Products.Models;

namespace GeekBurger.Products.Repository {
    public interface IStoreRepository {
        Store GetStoreByName(string name);
    }
}
