using E_Commerce_System_API.Application.DTOs.Orders;
using E_Commerce_System_API.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_System_API.Controllers
{
    
    [ApiController]
    [Route("[controller]/[action]")]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [Authorize(Roles = "Customer")]
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderDto orderDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var orderId = await _orderService.CreateOrderAsync(orderDto);
            return Ok("Your order has been placed!");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderAsync(id);
            if (order is null) return NotFound();
            return Ok(order);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllAsync();
            return Ok(orders);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromQuery] string status)
        {
            var updated = await _orderService.UpdateStatusAsync(id, status);
            return updated ? Ok("Order status is updated!") : NotFound();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Cancel(int id)
        {
            var cancelled = await _orderService.CancelAsync(id);
            return cancelled ? Ok("Order status is cancelled") : NotFound();
        }
    }
}
