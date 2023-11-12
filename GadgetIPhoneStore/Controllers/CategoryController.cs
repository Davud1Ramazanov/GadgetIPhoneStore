using GadgetIPhoneStore.Configuration;
using GadgetIPhoneStore.LocalControllers;
using GadgetIPhoneStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GadgetIPhoneStore.Controllers
{
    [ApiController, Authorize, Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        protected readonly ICategoryController _localController;

        public CategoryController(ICategoryController categoryLocalController)
        {
            _localController = categoryLocalController;
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create(Category category)
        {
            var result = await _localController.Add(category);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update(Category category)
        {
            var result = await _localController.Update(category);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("Remove")]
        public async Task<IActionResult> Remove(Category t)
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
    }
}
