using Shop.Data;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext db;

        public ProductRepository(AppDbContext db)
        {
            this.db = db;
        }
     
        public IEnumerable<Product> GetProducts()
        {
            return db.AppProducts.ToList();
        }
        public void DeleteProduct(int id)
        {
            var product = db.AppProducts.SingleOrDefault(m => m.Id == id);
            db.AppProducts.Remove(product);
            db.SaveChanges();
        }

        public void UpdateProduct(Product prodToUpdate)
        { 
            db.Update(prodToUpdate);
            db.SaveChanges();
        }

        void IProductRepository.CreateProduct(Product newProduct)
        {
            db.Add(newProduct);
            db.SaveChanges();
        }

        public Product GetProduct(int id)
        {
            var product = db.AppProducts.SingleOrDefault(m => m.Id == id);
            return product;
        }

        public int GetCount()
        {
            var count = db.AppProducts.Count();
            return count;
        }
    }
    }

