using ByteStore.Domain.Abstractions.Constants;
using ByteStore.Domain.Abstractions.Result;
using ByteStore.Domain.Repositories;
using BytStore.Application.DTOs.Payment;
using BytStore.Application.Helpers;
using BytStore.Application.IServices;
using Microsoft.Extensions.Options;
using Stripe;

namespace ByteStore.Persistance.Services
{
    internal class PaymentService : IPaymentService
    {
        private readonly StripeSettings stripeSettings;
        private readonly IShoppingCartRepository shoppingCartRepository;
        private readonly IUnitOfWork unitOfWork;

        public PaymentService(IOptionsMonitor<StripeSettings> stripeSettings, IShoppingCartRepository shoppingCartRepository, IUnitOfWork unitOfWork)
        {
            this.stripeSettings = stripeSettings.CurrentValue;
            this.shoppingCartRepository = shoppingCartRepository;
            this.unitOfWork = unitOfWork;
            StripeConfiguration.ApiKey = this.stripeSettings.SecretKey; // ✅ هنا يتسجل مره وحده
        }
        public async Task<Result2<PaymentIntentDto>> CreateOrUpdatePaymentIntentAsync(string cartId)
        {
            var cart = await shoppingCartRepository.GetCartAsync(cartId);
            if (cart == null) return Result2<PaymentIntentDto>.Failure(CartErrors.NotFound);


            var shippingPrice = StripeConsts.ShippingPrice;
            decimal subtotal = 0;
            foreach (var item in cart.CartItems)
            {
                var product = await unitOfWork.ProductRepository.GetByIdAsync(item.ProductId);
                if (product == null) return Result2<PaymentIntentDto>.Failure(ProductErrors.NotFound);

                subtotal += product.Price * item.Quantity; // السعر الحقيقي من DB
            }
            var total = subtotal + shippingPrice;


            var service = new PaymentIntentService();

            if (!string.IsNullOrEmpty(cart.PaymentIntentId))
            {
                var existingIntent = await service.GetAsync(cart.PaymentIntentId);

                if (existingIntent.Status == "succeeded")
                {
                    // Payment completed before → must create new one
                    cart.PaymentIntentId = null;
                    cart.ClientSecret = null;
                }
            }


            PaymentIntent intent;
            if (string.IsNullOrEmpty(cart.PaymentIntentId))
            {
                // No payment intent yet, create a new one
                var options = new PaymentIntentCreateOptions
                {
                    // Stripe requires the amount in the smallest currency unit (e.g., cents)
                    // So, for $15.50, you must provide 1550.
                    Amount = (long)(total * 100),
                    Currency =StripeConsts.Currency, // Change to your currency
                    PaymentMethodTypes = new List<string> { "card" },
                };
                intent = await service.CreateAsync(options);

                // Update our basket with the new Stripe info
                cart.PaymentIntentId = intent.Id;
                cart.ClientSecret = intent.ClientSecret;
            }
            else
            {
                // A payment intent already exists, so update it
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = (long)(total * 100),
                };
                intent = await service.UpdateAsync(cart.PaymentIntentId, options);
            }

            // Save the changes to our database
            var result=await shoppingCartRepository.SaveCartAsync(cart);
            if(!result)
                return Result2<PaymentIntentDto>.Failure(CartErrors.SaveFailed);

            var paymentIntentDto = new PaymentIntentDto
            {
                ClientSecret = intent.ClientSecret,
                PaymentIntentId = intent.Id,
                Status = intent.Status,
                Amount = intent.Amount,
                Currency = intent.Currency
            };

            return Result2<PaymentIntentDto>.Success(paymentIntentDto);
        }

    }
}
