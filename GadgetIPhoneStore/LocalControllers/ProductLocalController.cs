using GadgetIPhoneStore.Configuration;
using GadgetIPhoneStore.Context;
using GadgetIPhoneStore.Models;
using Microsoft.EntityFrameworkCore;

namespace GadgetIPhoneStore.LocalControllers
{
    public class ProductLocalController : IProductController
    {
        protected readonly DbContextClass _dbContextClass;

        public ProductLocalController(DbContextClass dbContextClass)
        {
            _dbContextClass = dbContextClass;   
        }

        public Task<List<Product>> Add(Product t)
        {
            var item = _dbContextClass.Products.FirstOrDefault(x => x.Name.Equals(t.Name));
            if (item == null)
            {
                _dbContextClass.Products.Add(t);
                _dbContextClass.SaveChanges();
            }
            return _dbContextClass.Products.ToListAsync();
        }

        public Task<List<Product>> Delete(Product t)
        {
            var item = _dbContextClass.Products.FirstOrDefault(x => x.ProductId.Equals(t.ProductId));
            if (item != null)
            {
                _dbContextClass.Products.Remove(item);
                _dbContextClass.SaveChanges();
            }
            return _dbContextClass.Products.ToListAsync();
        }

        public Task<List<Product>> Select()
        {
            return _dbContextClass.Products.ToListAsync();
        }

        public Task<List<Product>> Update(Product t)
        {
            var item = _dbContextClass.Products.FirstOrDefault(x => x.ProductId.Equals(t.ProductId));
            if (item == null)
            {
                item.CategoryId = t.CategoryId;
                item.Name = t.Name;
                item.Description = t.Description;
                item.Price = t.Price;
                item.Pixel = t.Pixel;
                item.Color = t.Color;
                item.Quantity = t.Quantity; 
                item.Image = t.Image;
                _dbContextClass.SaveChanges();
            }
            return _dbContextClass.Products.ToListAsync();
        }

        public async Task<List<Product>> FindByCategoryId(int categoryId)
        {
            List<Product> products = await _dbContextClass.Products.ToListAsync();
            if (categoryId > 0)
            {
                products = products.Where(x => x.CategoryId == categoryId).ToList();
            }
            return products;
        }

        public async Task<List<Product>> FindByProductId(int productId)
        {
            List<Product> products = await _dbContextClass.Products.ToListAsync();
            if (productId > 0)
            {
                products = products.Where(x => x.ProductId == productId).ToList();
            }
            return products;
        }

        public async Task<List<Product>> FindByProductName(string name)
        {
            List<Product> products = await _dbContextClass.Products.ToListAsync();
            if (!string.IsNullOrEmpty(name.ToLower()))
            {
                products = products.Where(x => x.Name.ToLower().Contains(name.ToLower())).ToList();
            }
            return products;
        }
    }
}
