using BytStore.Application.DTOs.Product;
using FluentValidation;

namespace BytStore.Application.Validators.Product
{
    internal class ProductUpdateDtoValidator:AbstractValidator<ProductUpdateDto>
    {
        public ProductUpdateDtoValidator()
        {
            // Reuse rules from ProductCreateDtoValidator
            Include(new ProductCreateDtoValidator());

            // Add validation for Id
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Product Id must be a valid id.");
        }
    }
    }
