using ByteStore.Domain.Abstractions.Constants;
using BytStore.Application.DTOs.Product;
using FluentValidation;

namespace BytStore.Application.Validators.Product
{
    internal class ProductImageCreateDtoValidator:AbstractValidator<ProductImageCreateDto>
    {
        private readonly string[] _allowedExtensions = [".jpg", ".jpeg", ".png", ".webp"];

        public ProductImageCreateDtoValidator()
        {
            RuleFor(x => x.Image)
                .NotNull().WithMessage("Image is required.")
                .Must(file => file!.Length > 0).WithMessage("Image file cannot be empty.")
                .Must(file => _allowedExtensions.Contains(Path.GetExtension(file!.FileName).ToLower()))
                .WithMessage($"Only {string.Join(", ",FileSettings.AllowedExtensions)} formats are allowed.")
                .Must(file => file!.Length <= FileSettings.MaxFileSizeInBytes)
                .WithMessage("Image size must not exceed 5 MB.");
        }
    }
}
