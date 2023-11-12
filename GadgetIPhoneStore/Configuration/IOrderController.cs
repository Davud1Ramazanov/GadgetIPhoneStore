using GadgetIPhoneStore.Models;

namespace GadgetIPhoneStore.Configuration
{
    public interface IOrderController : IController<Order>
    {
        public Task<List<Order>> FindByOrderId(int orderId);
    }
}
