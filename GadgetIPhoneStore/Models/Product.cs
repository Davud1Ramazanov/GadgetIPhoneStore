namespace GadgetIPhoneStore.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public int Pixel { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; } 
    }
}
