namespace BytStore.Application.DTOs.Order
{
    public class OrderItemDto
    {
        public class OrderItem
        {
            public int Quantity { get; set; }
            public int ProductId { get; set; }
        }
    }
}
