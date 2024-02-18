using Catalog.Core.Entities;

namespace Catalog.Core.Repositories;

/// <summary>
/// Todo: 2.7.1 Create repository interface
/// </summary>
public interface IBrandRepository
{
    Task<IEnumerable<ProductBrand>> GetAllBrands();
}