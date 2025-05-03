using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using E_Commerce_System_API.Application.Interfaces;
using E_Commerce_System_API.Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;

namespace E_Commerce_System_API.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IDapperContext _context;

        public OrderRepository(IDapperContext context)
        {
            _context = context;
        }

        public Task<bool> CancelOrderAsync(long id) =>
              UpdateOrderStatusAsync(id, "Cancelled");

        public async Task<int> CreateOrderAsync(Order order, List<OrderProduct> orderProducts)
        {

            using var conn = _context.CreateConnection();
            conn.Open();
            using var transaction = conn.BeginTransaction();

            var orderQuery = @"INSERT INTO Orders ( UserId, TotalAmount, OrderDate, Status) 
                         VALUES (@UserId, @TotalAmount, @OrderDate, @Status); 
                         SELECT last_insert_rowid();";

            var orderId = await conn.ExecuteScalarAsync<int>(orderQuery, order, transaction);

            var orderProductQuery = @"INSERT INTO OrderProducts ( OrderId, ProductId, Quantity) 
                                VALUES ( @OrderId, @ProductId, @Quantity);";

            foreach (var item in orderProducts)
            {
                await conn.ExecuteAsync(orderProductQuery, new
                {
                    OrderId = orderId,
                    item.ProductId,
                    item.Quantity
                }, transaction);
            }

            transaction.Commit();
            return orderId;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            using var conn = _context.CreateConnection();

            var getOrdersQuery = @" SELECT  o.Id, o.UserId, o.TotalAmount, o.OrderDate, o.Status,
                p.Id AS ProductId, p.Name, p.Price, op.Quantity, op.OrderId
                FROM Orders o
                JOIN OrderProducts op ON o.Id = op.OrderId
                JOIN Products p ON op.ProductId = p.Id
                ORDER BY o.Id";

            var orderDict = new Dictionary<int, Order>();

            var orders = await conn.QueryAsync<Order, OrderProductDetail, Order>(
                getOrdersQuery,
                (order, product) =>
                {
                    if (!orderDict.TryGetValue((int)order.Id, out var currentOrder))
                    {
                        currentOrder = order;
                        currentOrder.OrderProducts = new List<OrderProductDetail>();
                        orderDict.Add((int)currentOrder.Id, currentOrder);
                    }

                    currentOrder.OrderProducts.Add(product);
                    return currentOrder;
                },
                splitOn: "ProductId"
            );

            return orderDict.Values;
        }

        public async Task<Order> GetOrderByIdAsync(long id)
        {
            var getOrderQuery = "SELECT * FROM Orders WHERE Id = @Id;";
            using var conn = _context.CreateConnection();
            var order = await conn.QueryFirstOrDefaultAsync<Order>(getOrderQuery, new { Id = id });

            if (order == null) return null;

            var orderProductsQuery = @"SELECT p.Id AS ProductId, p.Name,p.Price, op.Quantity
                FROM OrderProducts op
                JOIN Products p ON op.ProductId = p.Id
                WHERE op.OrderId = @OrderId";

            var items = await conn.QueryAsync<OrderProductDetail>(orderProductsQuery, new { OrderId = id });
            order.OrderProducts = items.ToList();

            return order;
        }

        public async Task<bool> UpdateOrderStatusAsync(long id, string status)
        {
            var updateStatusQuery = "UPDATE Orders SET Status = @Status WHERE Id = @Id";
            using var conn = _context.CreateConnection();
            var affected = await conn.ExecuteAsync(updateStatusQuery, new { Id = id, Status = status });
            return affected > 0;
        }
        public async Task<List<OrderProduct>> GetOrderProductsAsync(long orderId)
        {
            var orderProductsQuery = "SELECT * FROM OrderProducts WHERE OrderId = @OrderId";
            using var conn = _context.CreateConnection();
            var items = await conn.QueryAsync<OrderProduct>(orderProductsQuery, new { OrderId = orderId });
            return items.ToList();
        }

        public async Task<IEnumerable<Order>> GetOrdersByStatusAsync(string status)
        {
            var getstatusQuery = "SELECT * FROM Orders WHERE Status = @Status";
            using var conn = _context.CreateConnection();
            return await conn.QueryAsync<Order>(getstatusQuery, new { Status = status });
        }
    }
}
