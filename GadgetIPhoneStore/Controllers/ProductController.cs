using GadgetIPhoneStore.Configuration;
using GadgetIPhoneStore.LocalControllers;
using GadgetIPhoneStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GadgetIPhoneStore.Controllers
{
    [ApiController, Authorize, Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        protected readonly IProductController _localController;
        protected readonly ICategoryController _categoryController;
        public ProductController(IProductController productLocal, ICategoryController categoryController)
        {
            _localController = productLocal;
            _categoryController = categoryController;

        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create(Product product)
        {
            var result = await _localController.Add(product);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update(Product product)
        {
            var result = await _localController.Update(product);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("Remove")]
        public async Task<IActionResult> Remove(Product t)
        {
            var result = await _localController.Delete(t);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("Select")]
        public async Task<IActionResult> Select()
        {
            var result = await _localController.Select();
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("FindByCategoryId")]
        public async Task<IActionResult> FindByCategoryId(int categoryId)
        {
            var result = await _localController.FindByCategoryId(categoryId);
            if(result != null)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("FindByProductId")]
        public async Task<IActionResult> FindByProductId(int productId)
        {
            var result = await _localController.FindByProductId(productId);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("FindByCategoryAndProductName")]
        public async Task<IActionResult> FindByCategoryAndProductName(int categoryId, string name)
        {
            var result = await _localController.FindByCategoryAndProductName(categoryId, name);
            {
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("FindByProductName")]
        public async Task<IActionResult> FindByProductName(string name)
        {
            var result = await _localController.FindByProductName(name);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("SelectProductByCategory")]
        public async Task<IActionResult> SelectProductByCategory()
        {
            var products = await _localController.Select();
            if (products != null)
            {
                var result = new List<object>();
                foreach (var product in products)
                {
                    var category = _categoryController.Select().Result.FirstOrDefault(x => x.CategoryId.Equals(product.CategoryId));
                    if(category != null)
                    {
                        result.Add(new { ProductId = product.ProductId, CategoryName = category.Name, CategoryId = category.CategoryId, Image = product.Image, Name = product.Name, Color = product.Color, Pixel = product.Pixel, Description = product.Description, Quantity = product.Quantity, Price = product.Price });
                    }
                }
                return Ok(result);
            }
            return NotFound();
        }
    }
}
