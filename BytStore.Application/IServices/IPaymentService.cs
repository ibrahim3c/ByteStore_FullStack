using ByteStore.Domain.Abstractions.Result;
using ByteStore.Domain.Entities;
using BytStore.Application.DTOs.Payment;

namespace BytStore.Application.IServices
{
    public interface IPaymentService
    {
        Task<Result2<PaymentIntentDto>> CreateOrUpdatePaymentIntentAsync(string customerId);
    }
}
