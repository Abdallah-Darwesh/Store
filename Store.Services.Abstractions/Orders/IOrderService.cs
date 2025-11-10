using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.Dtos.Orders;
using Store.Services.Orders; 
namespace Store.Services.Abstractions.Orders
{
    public interface IOrderService
    {
        Task<OrderResponse> CreateOrderAsync(OrderRequest request, string userEmail);
        Task<IEnumerable<DeliveryMethodResponse>> GetOrdersForUserAsync();
        Task<OrderResponse?> GetOrderByIdAsync(Guid orderId, string userEmail);
        Task<IEnumerable<OrderResponse?>> GetOrdersByUserAsync(string userEmail);
    }
}
