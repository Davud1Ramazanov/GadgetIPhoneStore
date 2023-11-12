using GadgetIPhoneStore.Models;

namespace GadgetIPhoneStore.Configuration
{
    public interface IProductController : IController<Product>
    {
       public Task<List<Product>> FindByCategoryId(int categoryId);
       public Task<List<Product>> FindByProductId(int productId);
       public Task<List<Product>> FindByProductName(string name);
    }
}
