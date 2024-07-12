namespace bakery.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public string OrderEmail { get; set; }
        public string ShippingAddress { get; set; }
        public decimal UnitPrice { get; set; }
        public bool SubscribeNewsLetter { get; set; }
    }
}
