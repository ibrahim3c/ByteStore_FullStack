using ByteStore.Application.DTOs.Payment;
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

        [HttpPost("payment-intent")]
        public async Task<IActionResult> CreateOrUpdatePaymentIntent(PaymentIntentRequestDto dto)
        {
            var result = await serviceManager.PaymentService.CreateOrUpdatePaymentIntentAsync(dto.CartId);

            if (!result.IsSuccess)
                return BadRequest(result.Error); // ترجع errors لو حصل fail

            return Ok(result.Value); // ترجع PaymentIntentDto لو نجح
        }


    }
}
