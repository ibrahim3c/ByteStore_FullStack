using ByteStore.Domain.Abstractions.Shared;
using ByteStore.Domain.Entities;

namespace BytStore.Application.Specifications
{
    public class ProductWithoutPaginationSpecification : Specification<Product>
    {
        public ProductWithoutPaginationSpecification(ProductParameters productParams)
             : base(p =>
                 (string.IsNullOrEmpty(productParams.SearchTerm) ||
                  p.Name.ToLower().Contains(productParams.SearchTerm.ToLower())) &&

                 (productParams.MinPrice == 0 || p.Price >= productParams.MinPrice) &&
                 (productParams.MaxPrice == decimal.MaxValue || p.Price <= productParams.MaxPrice) &&

                 (string.IsNullOrEmpty(productParams.Category) || p.Category.Name == productParams.Category) &&
                 (string.IsNullOrEmpty(productParams.Brand) || p.Brand.Name == productParams.Brand)
             )
        {
            // Include relations
            AddInclude(p => p.Category);
            AddInclude(p => p.Brand);
            AddInclude(p => p.Images);

            // Sorting
            if (!string.IsNullOrEmpty(productParams.OrderBy))
            {
                switch (productParams.OrderBy.ToLower())
                {
                    case "price":
                        AddOrderBy(p => p.Price);
                        break;
                    case "price_desc":
                        AddOrderByDescending(p => p.Price);
                        break;
                    case "brand":
                        AddOrderBy(p => p.Brand.Name);
                        break;
                    case "category":
                        AddOrderBy(p => p.Category.Name);
                        break;
                    default:
                        AddOrderBy(p => p.Name);
                        break;
                }
            }
            else
            {
                AddOrderBy(p => p.Name); // default
            }
        }
    }
}
