using Catalog.Core.Entities;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Data;

/// <summary>
/// Todo: 2.9.1 Setup Catalog Context Interface 
/// </summary>
public interface ICatalogContext
{
    IMongoCollection<Product> Products { get; }
    IMongoCollection<ProductBrand> Brands { get; }
    IMongoCollection<ProductType> Types { get; }
}