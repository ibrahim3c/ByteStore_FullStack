using BytStore.Application.DTOs.ShoppingCart;
using FluentValidation;

namespace BytStore.Application.Validators.ShoppingCart
{
    public class ShoppingCartValidator : AbstractValidator<ShoppingCartDto>
    {
        public ShoppingCartValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty().WithMessage("CustomerId is required.");

            RuleFor(x => x.CartItems)
                .NotNull().WithMessage("CartItems cannot be null.")
                .Must(items => items.Any()).WithMessage("Cart must have at least one item.");

            RuleForEach(x => x.CartItems)
                .SetValidator(new CartItemValidator());
        }
    }

}
