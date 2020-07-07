using Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Repositories
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetProducts();
        void CreateProduct(Product newProduct);
        void UpdateProduct(Product prodToUpdate);
        void DeleteProduct(int id);
       Product GetProduct(int id);
        int GetCount();

    }
}
