using GeekBurger.Products.Model;

namespace GeekBurger.Products.Repository.Interface {
    public interface IProductsRepository {
        IEnumerable<Product> GetProductsByStoreName(string storeName);
        List<Item> GetFullListOfItems();
        Product GetProductById(Guid productId);
        bool Add(Product product);
        void Save();
    }

}
