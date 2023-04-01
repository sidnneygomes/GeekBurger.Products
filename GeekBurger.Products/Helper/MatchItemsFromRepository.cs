﻿using AutoMapper;
using GeekBurger.Products.Contract;
using GeekBurger.Products.Model;
using GeekBurger.Products.Repository;
using GeekBurger.Products.Repository.Interface;
using System;
using System.Linq;

namespace GeekBurger.Products {
    public class MatchItemsFromRepository : IMappingAction<ItemToUpsert, Item> {
        private IProductsRepository _productRepository;
        public MatchItemsFromRepository(IProductsRepository
            productRepository) {
            _productRepository = productRepository;
        }

        public void Process(ItemToUpsert source, Item destination) {
            var fullListOfItems = _productRepository.GetFullListOfItems();

            var itemFound = fullListOfItems?
                .FirstOrDefault(item => item.Name
                .Equals(source.Name,
                    StringComparison.InvariantCultureIgnoreCase));

            if (itemFound != null)
                destination.ItemId = itemFound.ItemId;
            else
                destination.ItemId = Guid.NewGuid();
        }
    }

}