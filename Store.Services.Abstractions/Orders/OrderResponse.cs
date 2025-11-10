using Shared.Dtos.Orders;

namespace Store.Services.Abstractions.Orders
{
    public class OrderResponse
    {
        public Guid Id { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public string UserEmail { get; set; }
        public OrdersAddressDto  shippingAddress { get; set; }

        public string DeliveryMethod { get; set; }
        public int DeliveryMethodId { get; set; }
        public ICollection<OrderItemDto> OrderItems { get; set; }
        public decimal subTotal { get; set; }

        public decimal Total { get; set; }
    }
}