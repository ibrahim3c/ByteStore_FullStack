using ByteStore.Domain.Entities;
using ByteStore.PresentationLayer.Controllers;
using BytStore.Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace ByteStore.Presentation.Controllers
{
    public class ShoppingCartsController : BaseController
    {
        public ShoppingCartsController(IServiceManager serviceManager) : base(serviceManager)
        {
        }

        // GET: api/shoppingcarts/{customerId}
        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetCart(string customerId)
        {
            var result = await serviceManager.ShoppingCartService.GetCartAsync(customerId);

            if (!result.IsSuccess)
                return NotFound(result.Error);

            return Ok(result.Value);
        }

        // POST: api/shoppingcarts
        [HttpPost]
        public async Task<IActionResult> SaveCart([FromBody] ShoppingCart cart)
        {
            var result = await serviceManager.ShoppingCartService.SaveCartAsync(cart);

            if (!result.IsSuccess)
                return BadRequest(result.Error);
            return Ok();
        }

        // DELETE: api/shoppingcarts/{customerId}
        [HttpDelete("{customerId}")]
        public async Task<IActionResult> ClearCart(string customerId)
        {
            var result = await serviceManager.ShoppingCartService.ClearCartAsync(customerId);

            if (!result.IsSuccess)
                return BadRequest(result.Error);
            
            return NoContent();
        }
    }
}
