namespace ByteStore.Domain.Abstractions.Result
{
    public sealed record Error(string Code, string? Description = null)
    {
        public static Error None = new(string.Empty, string.Empty);
    }
    public static class CategoryErrors
    {
        public static readonly Error CategoryNotFound = new("Category.NotFound", "The Category not found");
    }
    public static class BrandErrors
    {
        public static readonly Error BrandNotFound = new("Brand.NotFound", "The Brand not found");
    }
    public static class AddressErrors
    {
        public static readonly Error NotFound =
            new("Address.NotFound", "The address was not found");

        public static readonly Error InvalidType =
            new("Address.InvalidType", "The provided address type is invalid");
    }
    public static class CustomerErrors
    {
        public static readonly Error NotFound =
            new("Customer.NotFound", "The customer was not found");
    }
    public static class UserErrors
    {
        public static readonly Error NotFound =
            new("User.NotFound", "The User was not found");
        public static readonly Error UpdateFailed =
            new("User.UpdateFailed", "Failed to update the user");
        public static readonly Error AddFailed =
    new("User.AddFailed", "Failed to add the user");
        public static readonly Error DeleteFailed =
            new("User.DeleteFailed", "Failed to delete the user");
        public static readonly Error RequiredEmail =
    new("User.RequiredEmail", "The Email is required");

    }
    public static class FileErrors
    {
        public static readonly Error Empty =
            new("File.Empty", "The file is empty.");

        public static readonly Error OnlyImage =
            new("File.OnlyImage", "Only image files allowed.");

        public static readonly Error FileIdNotFound =
            new("File.FileIdNotFound", "FileId not found");
    }
    public static class ProductErrors
    {
        public static readonly Error NotFound =
            new("Product.NotFound", "The product was not found");

        public static readonly Error ImageNotFound =
            new("ProductImage.NotFound", "The product image was not found");

        public static readonly Error ReviewNotFound =
            new("ProductReview.NotFound", "The review not found");

        public static readonly Error InvalidCategory =
            new("Product.InvalidCategory", "The provided category is invalid");

        public static readonly Error InvalidBrand =
            new("Product.InvalidBrand", "The provided brand is invalid");
    }
    public static class AuthErrors
    {
        public static readonly Error InvalidCredentials =
            new("Auth.InvalidCredentials", "Email or password is incorrect");

        public static readonly Error EmailNotConfirmed =
            new("Auth.EmailNotConfirmed", "Email needs to be confirmed before login");

        public static readonly Error UserNotFound =
            new("Auth.UserNotFound", "The specified user was not found");

        public static readonly Error InvalidToken =
            new("Auth.InvalidToken", "The provided token is invalid");

        public static readonly Error EmailConfirmationFailed =
            new("Auth.EmailConfirmationFailed", "Email confirmation failed");

        public static readonly Error PasswordResetFailed =
            new("Auth.PasswordResetFailed", "Error resetting password");

        public static readonly Error InvalidResetRequest =
            new("Auth.InvalidResetRequest", "UserId and code are required");
    }
    public static class CartErrors
    {
        public static readonly Error NotFound =
            new("Cart.NotFound", "The shopping cart was not found");

        public static readonly Error ItemNotFound =
            new("Cart.ItemNotFound", "The cart item was not found");

        public static readonly Error AlreadyExists =
            new("Cart.AlreadyExists", "The shopping cart already exists");

        public static readonly Error InvalidCustomer =
            new("Cart.InvalidCustomer", "The customer for this cart is invalid");

        public static readonly Error SaveFailed =
            new("Cart.SaveFailed", "Failed to save the shopping cart");

        public static readonly Error ClearFailed =
            new("Cart.ClearFailed", "Failed to clear the shopping cart");
    }
    public static class OrderErrors
    {
        public static readonly Error NotFound =
            new("Order.NotFound", "The order was not found");

        public static readonly Error InvalidStatus =
            new("Order.InvalidStatus", "The provided order status is invalid");

        public static readonly Error SaveFailed =
            new("Order.SaveFailed", "Failed to save the order");

        public static readonly Error ItemNotFound =
            new("Order.ItemNotFound", "The order item was not found");

        public static readonly Error InvalidCustomer =
            new("Order.InvalidCustomer", "The customer for this order is invalid");

        public static readonly Error InsufficientStock =
       new("Order.InsufficientStock", "Not enough stock for one or more items");

        public static readonly Error InvalidCustomerId =
        new("Order.InvalidCustomerId", "The provided customer ID is not valid.");
        public static readonly Error BillingAddressNotFound =
      new("Order.BillingAddressNotFound", "The billing address was not found.");

        public static readonly Error ShippingAddressNotFound =
            new("Order.ShippingAddressNotFound", "The shipping address was not found.");
    }
    public static class RoleErrors
    {
        public static readonly Error NotFound =
            new("Role.NotFound", "The role was not found");

        public static readonly Error AlreadyExists =
            new("Role.AlreadyExists", "The role already exists");

        public static readonly Error CreateFailed =
            new("Role.CreateFailed", "Failed to create the role");

        public static readonly Error UpdateFailed =
            new("Role.UpdateFailed", "Failed to update the role");

        public static readonly Error DeleteFailed =
            new("Role.DeleteFailed", "Failed to delete the role");

        public static readonly Error InvalidRoleName =
            new("Role.InvalidRoleName", "The role name is invalid");
    }
}
