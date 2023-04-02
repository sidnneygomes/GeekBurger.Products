using AutoMapper;
using GeekBurger.Products.Contract;
using GeekBurger.Products.Model;
using GeekBurger.Products.Repository;
using GeekBurger.Products.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace GeekBurger.Products.Controllers {

    [Route("api/products")]
    public class ProductsController : Controller {

        private IProductsRepository _productsRepository;
        private IMapper _mapper;
        //private IList<Product> Products = new List<Product>();

        public ProductsController(IProductsRepository productsRepository, IStoreRepository storeRepository, IMapper mapper) {

            _productsRepository = productsRepository;
            _mapper = mapper;


            //var paulistaStore = "Paulista";
            //var morumbiStore = "Morumbi";

            //var beef = new Item { ItemId = Guid.NewGuid(), Name = "beef" };
            //var pork = new Item { ItemId = Guid.NewGuid(), Name = "pork" };
            //var mustard = new Item { ItemId = Guid.NewGuid(), Name = "mustard" };
            //var ketchup = new Item { ItemId = Guid.NewGuid(), Name = "ketchup" };
            //var bread = new Item { ItemId = Guid.NewGuid(), Name = "bread" };
            //var wBread = new Item { ItemId = Guid.NewGuid(), Name = "whole bread" };

            //Products = new List<Product>()
            //{
            //    new Product { ProductId = Guid.NewGuid(), Name = "Darth Bacon",
            //        Image = "hamb1.png", StoreName = paulistaStore,
            //        Items = new List<Item> {beef, ketchup, bread }
            //    },
            //    new Product { ProductId = Guid.NewGuid(), Name = "Cap. Spork",
            //        Image = "hamb2.png", StoreName = paulistaStore,
            //        Items = new List<Item> { pork, mustard, wBread }
            //    },
            //    new Product { ProductId = Guid.NewGuid(), Name = "Beef Turner",
            //        Image = "hamb3.png", StoreName = morumbiStore,
            //        Items = new List<Item> {beef, mustard, bread }
            //    }
            //};
        }


        //[HttpGet("{storename}")]
        //public IActionResult GetProductsByStoreName(string storeName) {
        //    var productsByStore = Products.Where(product =>
        //        product.StoreName == storeName).ToList();

        //    if (productsByStore.Count <= 0)
        //        return NotFound();

        //    return Ok(productsByStore);
        //}

        [HttpGet()]
        public IActionResult GetProductsByStoreName([FromQuery] string storeName) {
            var productsByStore = _productsRepository.GetProductsByStoreName(storeName).ToList();

            if (productsByStore.Count <= 0)
                return NotFound("Nenhum dado encontrado");

            var productsToGet = _mapper.Map<IEnumerable<ProductToGet>>(productsByStore);

            return Ok(productsToGet);
        }

        [HttpPost()]
        public IActionResult AddProduct([FromBody] ProductToUpsert productToAdd) {
            if (productToAdd == null)
                return BadRequest();

            var product = _mapper.Map<Product>(productToAdd);

            if (product.StoreId == Guid.Empty)
                return new Helper.UnprocessableEntityResult(ModelState);

            _productsRepository.Add(product);
            _productsRepository.Save();
            var productToGet = _mapper.Map<ProductToGet>(product);

            return CreatedAtRoute("GetProduct",
                new { id = productToGet.ProductId },
                productToGet);
        }

        [HttpGet("{id}", Name = "GetProduct")]
        public IActionResult GetProduct(Guid id) {
            var product = _productsRepository.GetProductById(id);
            var productToGet = _mapper.Map<ProductToGet>(product);

            return Ok(productToGet);
        }


    }

}
