namespace BytStore.Application.DTOs.ShoppingCart
{
     public class ShoppingCartDto
    {
        public string Id { get; set; }
        public List<CartItemDto> CartItems { get; set; } = new();

    }
}
