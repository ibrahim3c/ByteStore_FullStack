namespace ByteStore.Domain.Abstractions.Shared
{
    public class ProductParameters : RequestParameters
    {
        public ProductParameters() => OrderBy = "name";
        public decimal MinPrice { get; set; } = 0;
        public decimal MaxPrice { get; set; } = decimal.MaxValue;
        //public string? Category { get; set; }
        //public string? Brand { get; set; }


        public int? CategoryId { get; set; }
        public int? BrandId { get; set; }

        public bool ValidPriceRange => MaxPrice >= MinPrice;

        public string? SearchTerm { get; set; }
    }

}
