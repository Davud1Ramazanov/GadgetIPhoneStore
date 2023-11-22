using GadgetIPhoneStore.Configuration;
using GadgetIPhoneStore.Context;
using GadgetIPhoneStore.Models;
using GadgetIPhoneStore.Roles;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GadgetIPhoneStore.LocalControllers
{
    public class OrderLocalController : IOrderController
    {
        protected readonly DbContextClass _dbContextClass;
        protected readonly IHttpContextAccessor _httpContextAccessor;

        public OrderLocalController(DbContextClass dbContextClass, IHttpContextAccessor httpContextAccessor)
        {
            _dbContextClass = dbContextClass;
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetName()
        {
            return _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value;
        }

        public Task<List<Order>> Add(Order t)
        {
            var item = _dbContextClass.Orders.FirstOrDefault(x => x.ProductId.Equals(t.ProductId));
            var userName = GetName();
            var productItem = _dbContextClass.Products.FirstOrDefault(x => x.ProductId.Equals(t.ProductId));
            if(productItem == null)
            {
                throw new Exception("Product not found");
            }

            if (item != null && userName.Equals(item.Buyer))
            {
                item.Quantity += t.Quantity;
                item.DateOrder = DateTime.Now;
                productItem.Quantity -= t.Quantity;
            }

            else
            {
                productItem.Quantity -= t.Quantity;
                _dbContextClass.Orders.Add(new Order { ProductId = t.ProductId, Buyer = GetName(), Quantity = t.Quantity, Total = t.Total, DateOrder = t.DateOrder });
            }
            _dbContextClass.SaveChanges();
            return _dbContextClass.Orders.ToListAsync();
        }

        public Task<List<Order>> Delete(Order t)
        {
            var item = _dbContextClass.Orders.FirstOrDefault(x => x.OrderId.Equals(t.OrderId));
            var productItem = _dbContextClass.Products.FirstOrDefault(x => x.ProductId.Equals(t.ProductId));
            if (item != null)
            {
                productItem.Quantity += t.Quantity;
                _dbContextClass.Orders.Remove(item);
                _dbContextClass.SaveChanges();
            }
            return _dbContextClass.Orders.ToListAsync();
        }

        public Task<List<Order>> DeleteByProduct(Order t)
        {
            var item = _dbContextClass.Orders.FirstOrDefault(x => x.OrderId.Equals(t.OrderId));
            var productItem = _dbContextClass.Products.FirstOrDefault(x => x.ProductId.Equals(t.ProductId));
            if (item != null)
            {
                _dbContextClass.Orders.Remove(item);
                _dbContextClass.SaveChanges();
            }
            return _dbContextClass.Orders.ToListAsync();
        }

        public Task<List<Order>> Select()
        {
            return _dbContextClass.Orders.Where(x => x.Buyer.Equals(GetName())).ToListAsync();
        }

        public Task<List<Order>> SelectAllOrders()
        {
            return _dbContextClass.Orders.ToListAsync();
        }

        public Task<List<Order>> Update(Order t)
        {
            var item = _dbContextClass.Orders.FirstOrDefault(x => x.OrderId.Equals(t.OrderId));
            var productItem = _dbContextClass.Products.FirstOrDefault(x => x.ProductId.Equals(t.ProductId));
            if (item != null)
            {
                var oldQuantity = item.Quantity;
                item.Buyer = GetName();
                item.ProductId = t.ProductId;
                item.Total = t.Total;
                item.Quantity = t.Quantity;
                item.DateOrder = t.DateOrder;
                productItem.Quantity += oldQuantity - t.Quantity;
                _dbContextClass.SaveChanges();
            }
            return _dbContextClass.Orders.ToListAsync();
        }

        public async Task<List<Order>> FindByOrderId(int orderId)
        {
            List<Order> orders = await _dbContextClass.Orders.ToListAsync();
            if (orderId > 0)
            {
                orders = orders.Where(x => x.OrderId == orderId).ToList();
            }
            return orders;
        }
    }
}
