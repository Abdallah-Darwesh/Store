

using Shared.Dtos.Orders;

namespace Store.Services.Orders
{
    public class OrderRequest
    {
        public string BasketId { get; set; } = string.Empty;
        public OrdersAddressDto ShippingAddress { get; set; } = new OrdersAddressDto();

        public int DeliveryMethodId { get; set; }
    }
}
