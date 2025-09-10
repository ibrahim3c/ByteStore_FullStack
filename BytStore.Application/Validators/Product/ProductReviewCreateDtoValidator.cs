using BytStore.Application.DTOs.Product;
using FluentValidation;

namespace BytStore.Application.Validators.Product
{
    internal class ProductReviewCreateDtoValidator:AbstractValidator<ProductReviewCreateDto>
    {
        public ProductReviewCreateDtoValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty().WithMessage("CustomerId Required.");

            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5.");

            RuleFor(x => x.Comment)
                .NotEmpty().WithMessage("Comment is required.")
                .MaximumLength(1000).WithMessage("Comment must not exceed 1000 characters.");
        }
    }
}
