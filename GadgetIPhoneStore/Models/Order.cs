namespace GadgetIPhoneStore.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string Buyer { get; set; }
        public int Quantity { get; set; }
        public float Total { get; set; }
        public DateTime DateOrder { get; set; }
    }
}
