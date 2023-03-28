﻿using GeekBurger.Products.Models;
using Microsoft.EntityFrameworkCore;

namespace GeekBurger.Products.Repository {
    public class ProductsRepository : IProductsRepository {
        private ProductsDbContext _context;

        public ProductsRepository(ProductsDbContext context) {
            _context = context;
        }

        public IEnumerable<Product> GetProductsByStoreName(string storeName) {
            var products = _context.Products?
            .Where(product =>
                product.Store.Name.Equals(storeName,
                StringComparison.InvariantCultureIgnoreCase))
            .Include(product => product.Items);

            return products;
        }

        public bool Add(Product product) {
            product.ProductId = Guid.NewGuid();
            _context.Products.Add(product);
            return true;
        }

        public void Save() {
            _context.SaveChanges();
        }

    }


}
