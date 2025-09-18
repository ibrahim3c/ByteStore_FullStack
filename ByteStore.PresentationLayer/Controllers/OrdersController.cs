using ByteStore.Domain.Abstractions.Constants;
using ByteStore.Domain.Abstractions.Enums;
using ByteStore.PresentationLayer.Controllers;
using BytStore.Application.DTOs.Order;
using BytStore.Application.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ByteStore.Presentation.Controllers
{
    [Authorize]
    public class OrdersController : BaseController
    {
        public OrdersController(IServiceManager serviceManager) : base(serviceManager)
        {
        }


        // GET: api/orders
        [HttpGet]
        [Authorize(Roles = Roles.AdminRole)]
        public async Task<IActionResult> GetAllOrders()
        {
            var result = await serviceManager.OrderService.GetAllOrdersAsync();
            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return Ok(result.Value);
        }

        // GET: api/orders/paged?pageNumber=1&pageSize=10
        [HttpGet("paged")]
        [Authorize(Roles = Roles.AdminRole)]
        public async Task<IActionResult> GetAllOrdersPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await serviceManager.OrderService.GetAllOrderstsAsync(pageNumber, pageSize);
            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return Ok(result.Value);
        }

        // GET: api/orders/customer/{customerId}
        [HttpGet("customer/{customerId:guid}")]
        public async Task<IActionResult> GetCustomerOrders(Guid customerId)
        {
            var result = await serviceManager.OrderService.GetCustomerOrdersAsync(customerId);
            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return Ok(result.Value);
        }

        // GET: api/orders/{orderId}
        [HttpGet("{orderId:guid}")]
        public async Task<IActionResult> GetOrderById(Guid orderId)
        {
            var result = await serviceManager.OrderService.GetOrderByIdAsync(orderId);
            if (!result.IsSuccess)
                return NotFound(result.Error);

            return Ok(result.Value);
        }

        // POST: api/orders
        [HttpPost]
        [Authorize(Roles = Roles.AdminRole)]
        public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderDto dto)
        {
            var result = await serviceManager.OrderService.PlaceOrderAsync(dto);
            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return Ok(result);
        }

        // PUT: api/orders/{orderId}/status
        [HttpPut("{orderId:guid}/status")]
        [Authorize(Roles = Roles.AdminRole)]
        public async Task<IActionResult> UpdateOrderStatus(Guid orderId, [FromBody] OrderStatus newStatus)
        {
            var result = await serviceManager.OrderService.UpdateOrderStatusAsync(orderId, newStatus);
            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return Ok(result);
        }
    }
}
