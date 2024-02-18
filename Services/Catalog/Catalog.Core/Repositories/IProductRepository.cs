using Catalog.Core.Entities;
using Catalog.Core.Specs;

namespace Catalog.Core.Repositories;

/// <summary>
/// Todo: 2.7.1 Create repository interface
/// </summary>
public interface IProductRepository
{
    Task<Pagination<Product>> GetProducts(CatalogSpecParams catalogSpecParams);
    Task<Product> GetProduct(string id);
    Task<IEnumerable<Product>> GetProductByName(string name);
    Task<IEnumerable<Product>> GetProductByBrand(string name);
    Task<Product> CreateProduct(Product product);
    Task<bool> UpdateProduct(Product product);
    Task<bool> DeleteProduct(string id);
}