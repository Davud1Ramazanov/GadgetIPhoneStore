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
        public ProductController(IProductController productLocal)
        {
            _localController = productLocal;
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
    }
}
