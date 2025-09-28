using BytStore.Application.DTOs.Shared;

namespace BytStore.Application.DTOs.Product
{
    public class ProductParameters : RequestParameters
    {
        public decimal MinPrice { get; set; } = 0;
        public decimal MaxPrice { get; set; } = decimal.MaxValue;
        public string? Category { get; set; }
        public string? Brand { get; set; }

        public bool ValidPriceRange => MaxPrice >= MinPrice;

    
    }

}
