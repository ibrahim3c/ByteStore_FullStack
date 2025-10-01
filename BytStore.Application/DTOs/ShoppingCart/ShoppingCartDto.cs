namespace BytStore.Application.DTOs.ShoppingCart
{
     public class ShoppingCartDto
    {
        public string CustomerId { get; set; }
        public List<CartItemDto> CartItems { get; set; } = new();

    }
}
