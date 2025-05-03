using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_System_API.Application.Interfaces
{
    public interface INotificationService
    {
        Task NotifyLowStockAsync(long productId, int currentStock);
        Task SendEmailAsync(int userId, string message);
    }
}
