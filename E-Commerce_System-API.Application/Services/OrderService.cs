using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using E_Commerce_System_API.Application.DTOs.Orders;
using E_Commerce_System_API.Application.Interfaces;
using E_Commerce_System_API.Domain.Entities;

namespace E_Commerce_System_API.Application.Services
{
    public class OrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;

        public OrderService(IOrderRepository orderRepository, IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        public async Task<int> CreateOrderAsync(OrderDto orderDto)
        {
            decimal total = 0;
            var orderProducts = new List<OrderProduct>();

            foreach (var orderProduct in orderDto.Products)
            {
                var product = await _productRepository.GetByIdAsync(orderProduct.ProductId);
                if (product == null || product.Stock < orderProduct.Quantity)
                    throw new Exception($"Product ID {orderProduct.ProductId} is out of stock.");

                total += product.Price * orderProduct.Quantity;

                orderProducts.Add(new OrderProduct
                {
                    ProductId = orderProduct.ProductId,
                    Quantity = orderProduct.Quantity
                });
            }

            var order = new Order
            {
                UserId = orderDto.UserId,
                OrderDate = DateTime.UtcNow,
                Status = "Pending", //hard-coded for the moment
                TotalAmount = total
            };

            var orderId = await _orderRepository.CreateOrderAsync(order, orderProducts);

            foreach (var item in orderProducts)
            {
                await _productRepository.DecreaseStockAsync(item.ProductId, item.Quantity);
            }
            return orderId;
        }

        public Task<Order> GetOrderAsync(int id) => _orderRepository.GetOrderByIdAsync(id);

        public Task<IEnumerable<Order>> GetAllAsync() => _orderRepository.GetAllOrdersAsync();
        public Task<bool> UpdateStatusAsync(int id, string status) => _orderRepository.UpdateOrderStatusAsync(id, status);
        public async Task<bool> CancelAsync(int orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order is null || order.Status == "Cancelled")
                return false;

            var cancelled = await _orderRepository.CancelOrderAsync(orderId);
            if (!cancelled) return false;

            //if order is cancelled increase stock of the products 
            var orderProducts = await _orderRepository.GetOrderProductsAsync(orderId);
            foreach (var item in orderProducts)
            {
                await _productRepository.IncreaseStockAsync(item.ProductId, item.Quantity);
            }

            return true;
        }

    }
}
