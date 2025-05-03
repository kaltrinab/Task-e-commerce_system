using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce_System_API.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace E_Commerce_System_API.Application.Services
{
    public class OrderStatusService : BackgroundService
    {
        private readonly IServiceProvider _provider;
        private readonly ILogger<OrderStatusService> _logger;

        public OrderStatusService(IServiceProvider provider, ILogger<OrderStatusService> logger)
        {
            _provider = provider;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(" Order processing service started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _provider.CreateScope();
                var orderRepo = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
                var notification = scope.ServiceProvider.GetRequiredService<INotificationService>();

                var pendingOrders = await orderRepo.GetOrdersByStatusAsync("Pending");

                foreach (var order in pendingOrders)
                {
                    await orderRepo.UpdateOrderStatusAsync(order.Id, "Confirmed");

                    await notification.SendEmailAsync((int)order.UserId, $"Order #{order.Id} has been confirmed.");

                    _logger.LogInformation($"Processed order #{order.Id} (Confirmed).");
                }

                await Task.Delay(TimeSpan.FromSeconds(60), stoppingToken); // run every 30 seconds
            }

            _logger.LogInformation("Order processing service stopped.");
        }
    }
}
