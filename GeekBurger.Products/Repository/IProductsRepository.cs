using GeekBurger.Products.Models;

namespace GeekBurger.Products.Repository
{
    public interface IProductsRepository
    {
        IEnumerable<Product> GetProductsByStoreName(string storeName);

    }
}
