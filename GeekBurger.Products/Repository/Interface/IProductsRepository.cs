using GeekBurger.Products.Model;

namespace GeekBurger.Products.Repository.Interface {

    public interface IProductsRepository {
        Product GetProductById(Guid productId);
        List<Item> GetFullListOfItems();
        bool Add(Product product);
        bool Update(Product product);
        IEnumerable<Product> GetProductsByStoreName(string storeName);
        void Delete(Product product);
        void Save();
    }
}
