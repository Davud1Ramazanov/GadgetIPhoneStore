using GadgetIPhoneStore.Configuration;
using GadgetIPhoneStore.LocalControllers;
using GadgetIPhoneStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GadgetIPhoneStore.Controllers
{
    [ApiController, Authorize, Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        protected readonly IOrderController _localController;
        protected readonly IProductController _productController;
        public OrderController(IOrderController orderLocalController, IProductController productController)
        {
            _localController = orderLocalController;
            _productController = productController;
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create(Order order)
        {
            var result = await _localController.Add(order);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update(Order order)
        {
            var result = await _localController.Update(order);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("Remove")]
        public async Task<IActionResult> Remove(Order t)
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
        [Route("FindByOrderId")]
        public async Task<IActionResult> FindByOrderId(int orderId)
        {
            var result = await _localController.FindByOrderId(orderId);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("SelectOrderByProd")]
        public async Task<IActionResult> SelectOrderByProd()
        {
            var orders = await _localController.Select();
            if (orders != null && orders.Any())
            {
                var result = new List<object>();
                foreach (var order in orders)
                {
                    var product = _productController.Select().Result.FirstOrDefault(x => x.ProductId == order.ProductId);
                    if (product != null)
                    {
                        result.Add(new { OrderId = order.OrderId, ProductId = order.ProductId, ProductName = product.Name, ProductQuantity = product.Quantity, Image = product.Image, Buyer = order.Buyer, Quantity = order.Quantity, Total = order.Total, DateOrder = order.DateOrder });
                    }
                }
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }

    }
}
