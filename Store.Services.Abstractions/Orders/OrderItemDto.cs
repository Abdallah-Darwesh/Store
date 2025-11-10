namespace Store.Services.Abstractions.Orders
{
    public class OrderItemDto
    {
        public int productId { get; set; }
        public string productName { get; set; }
        public string PictureUrl { get; set; }


        public int price { get; set; }
        public int quantity { get; set; }

    }
}