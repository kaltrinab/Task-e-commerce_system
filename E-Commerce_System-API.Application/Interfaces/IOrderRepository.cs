using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce_System_API.Domain.Entities;

namespace E_Commerce_System_API.Application.Interfaces
{
    public interface IOrderRepository
    {
        Task<int> CreateOrderAsync(Order order, List<OrderProduct> orderProducts);
        Task<Order> GetOrderByIdAsync(long id);
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<bool> UpdateOrderStatusAsync(long id, string status);
        Task<bool> CancelOrderAsync(long id);
        Task<List<OrderProduct>> GetOrderProductsAsync(long orderId);
        Task<IEnumerable<Order>> GetOrdersByStatusAsync(string status);
    }
}
