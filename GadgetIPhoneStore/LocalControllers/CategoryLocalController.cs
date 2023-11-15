using GadgetIPhoneStore.Configuration;
using GadgetIPhoneStore.Context;
using GadgetIPhoneStore.Models;
using Microsoft.EntityFrameworkCore;

namespace GadgetIPhoneStore.LocalControllers
{
    public class CategoryLocalController : ICategoryController
    {
        protected readonly DbContextClass _dbContextClass;

        public CategoryLocalController(DbContextClass dbContextClass)
        {
            _dbContextClass = dbContextClass;
        }

        public Task<List<Category>> Add(Category t)
        {
            var item = _dbContextClass.Categories.FirstOrDefault(x => x.Name.Equals(t.Name));
            if (item == null) 
            { 
                _dbContextClass.Categories.Add(t);
                _dbContextClass.SaveChanges();
            }
            return _dbContextClass.Categories.ToListAsync();
        }

        public Task<List<Category>> Delete(Category t)
        {
            var item = _dbContextClass.Categories.FirstOrDefault(x => x.CategoryId.Equals(t.CategoryId));
            if (item != null)
            {
                _dbContextClass.Categories.Remove(item);
                _dbContextClass.SaveChanges();
            }
            return _dbContextClass.Categories.ToListAsync();
        }

        public Task<List<Category>> Select()
        {
            return _dbContextClass.Categories.ToListAsync();
        }

        public Task<List<Category>> Update(Category t)
        {
            var item = _dbContextClass.Categories.FirstOrDefault(x => x.CategoryId.Equals(t.CategoryId));
            if (item != null)
            {
                item.Name = t.Name;
                item.Image = t.Image;
                _dbContextClass.SaveChanges();
            }
            return _dbContextClass.Categories.ToListAsync();
        }
    }
}
