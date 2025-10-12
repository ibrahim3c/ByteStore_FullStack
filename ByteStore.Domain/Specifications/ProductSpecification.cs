using ByteStore.Domain.Abstractions.Shared;
using ByteStore.Domain.Entities;
using BytStore.Application.Specifications;

namespace ByteStore.Domain.Specifications
{
    public class ProductSpecification : Specification<Product>
    {
        public ProductSpecification(ProductParameters productParams)
            : base(p =>
                (string.IsNullOrEmpty(productParams.SearchTerm) ||
                 p.Name.ToLower().Contains(productParams.SearchTerm.ToLower())) &&

                (productParams.MinPrice == 0 || p.Price >= productParams.MinPrice) &&
                (productParams.MaxPrice == decimal.MaxValue || p.Price <= productParams.MaxPrice) &&

        //(string.IsNullOrEmpty(productParams.Category) || p.Category.Name == productParams.Category) &&
        //(string.IsNullOrEmpty(productParams.Brand) || p.Brand.Name == productParams.Brand)


        (!productParams.CategoryId.HasValue || p.CategoryId == productParams.CategoryId.Value) &&
        (!productParams.BrandId.HasValue || p.BrandId == productParams.BrandId.Value)
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
                    case "name":
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

            // Pagination
            ApplyPaging((productParams.PageNumber - 1) * productParams.PageSize, productParams.PageSize);
        }
    }


}
