using ByteStore.PresentationLayer.Controllers;
using BytStore.Application.DTOs.ShoppingCart;
using BytStore.Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace ByteStore.Presentation.Controllers
{
    //[Authorize]
    public class ShoppingCartsController : BaseController
    {
        public ShoppingCartsController(IServiceManager serviceManager) : base(serviceManager)
        {
        }

        //GET: api/shoppingcarts/{customerId}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCart(string id)
        {
            var result = await serviceManager.ShoppingCartService.GetCartAsync(id);

            if (!result.IsSuccess)
                return NotFound(result.Error);

            return Ok(result.Value);
        }

        //[HttpGet()]
        //public async Task<IActionResult> GetCart()
        //{

        //    string customerId = Request.Cookies[Keys.CartKey];
        //    if (string.IsNullOrEmpty(customerId))
        //    {
        //        customerId = Guid.NewGuid().ToString();
        //        var cookieOptions = new CookieOptions
        //        {
        //            IsEssential = true,
        //            Expires = DateTime.Now.AddDays(7)
        //        };
        //        Response.Cookies.Append(Keys.CartKey, customerId, cookieOptions);
        //    }

        //    var result = await serviceManager.ShoppingCartService.GetCartAsync(customerId);

        //    if (!result.IsSuccess)
        //        return NotFound(result.Error);

        //    return Ok(result.Value);
        //}


        // POST: api/shoppingcarts
        // for add and update
        [HttpPost]
        public async Task<IActionResult> SaveCart([FromBody] ShoppingCartDto cart)
        {
            // ensure buyerId exists
            //if (string.IsNullOrEmpty(cart.Id))
            //{
            //    cart.Id = Request.Cookies[Keys.CartKey] ?? Guid.NewGuid().ToString();
            //}
            //var result = await serviceManager.ShoppingCartService.SaveCartAsync(cart);
            var result = await serviceManager.ShoppingCartService.SaveCart2Async(cart);

            if (!result.IsSuccess)
                return BadRequest(result.Error);
            return Ok(result.Value);
        }

        // DELETE: api/shoppingcarts/{customerId}
        [HttpDelete("{id}")]
        public async Task<IActionResult> ClearCart(string id)
        {

            var result = await serviceManager.ShoppingCartService.ClearCartAsync(id);

            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return NoContent();
        }

        //[HttpDelete]
        //public async Task<IActionResult> ClearCart()
        //{
        //    string customerId = Request.Cookies[Keys.CartKey];

        //    if (string.IsNullOrEmpty(customerId))
        //        return BadRequest("No cart to clear.");

        //    var result = await serviceManager.ShoppingCartService.ClearCartAsync(customerId);

        //    if (!result.IsSuccess)
        //        return BadRequest(result.Error);

        //    // optional: ممكن كمان تمسح الـ cookie بعد المسح
        //    Response.Cookies.Delete(Keys.CartKey);
        //    return NoContent();
        //}

    }
}
