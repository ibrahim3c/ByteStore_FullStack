using ByteStore.Domain.Abstractions.Constants;
using ByteStore.PresentationLayer.Controllers;
using BytStore.Application.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ByteStore.Presentation.Controllers
{
    [Authorize(Roles = Roles.UserRole)]
    public class PaymentsController : BaseController
    {
        public PaymentsController(IServiceManager serviceManager) : base(serviceManager)
        {
        }

        [HttpPost("{customerId}")]
        public async Task<IActionResult> CreateOrUpdatePaymentIntent(string customerId)
        {
            var result = await serviceManager.PaymentService.CreateOrUpdatePaymentIntentAsync(customerId);

            if (!result.IsSuccess)
                return BadRequest(result.Error); // ترجع errors لو حصل fail

            return Ok(result.Value); // ترجع PaymentIntentDto لو نجح
        }
    }
}
