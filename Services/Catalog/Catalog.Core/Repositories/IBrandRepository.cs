using Catalog.Core.Entities;

namespace Catalog.Core.Repositories;

//Todo: 2.7.1 Create repository interface
public interface IBrandRepository
{
    Task<IEnumerable<ProductBrand>> GetAllBrands();
}