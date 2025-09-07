namespace BytStore.Application.DTOs.Order
{
    public class GetOrderItemDto
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; } // Snapshot of the price when ordered
        public string ProductName { get; set; }
        public int ProductId { get; set; } // useful reference

    }
}
