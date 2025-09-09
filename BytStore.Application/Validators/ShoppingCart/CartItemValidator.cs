using BytStore.Application.DTOs.ShoppingCart;
using FluentValidation;
namespace BytStore.Application.Validators.ShoppingCart
{

    public class CartItemValidator : AbstractValidator<CartItemDto>
    {
        public CartItemValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("ProductId must be greater than 0.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Product name is required.")
                .MaximumLength(100).WithMessage("Product name cannot exceed 100 characters.");

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Price must be greater than or equal to 0.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be at least 1.");

            RuleFor(x => x.ImageUrl)
                .NotEmpty().WithMessage("Image URL is required.")
                .MaximumLength(500).WithMessage("Image URL cannot exceed 500 characters.")
                .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
                .WithMessage("Image URL must be a valid URL.");

            RuleFor(x => x.BrandName)
                .MaximumLength(100).WithMessage("Brand name cannot exceed 100 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.BrandName));

            RuleFor(x => x.CategoryName)
                .NotEmpty().WithMessage("Category name is required.")
                .MaximumLength(100).WithMessage("Category name cannot exceed 100 characters.");
        }
    }

}
