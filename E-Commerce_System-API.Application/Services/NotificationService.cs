using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce_System_API.Application.Interfaces;

namespace E_Commerce_System_API.Application.Services
{
    public class NotificationService : INotificationService
    {
        public Task NotifyLowStockAsync(long productId, int currentStock)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Product ID {productId} has only {currentStock} items left.");
            Console.ResetColor();
            return Task.CompletedTask;
        }

        public Task SendEmailAsync(int userId, string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Email to User {userId}: {message}");
            Console.ResetColor();
            return Task.CompletedTask;
        }
    }
}
