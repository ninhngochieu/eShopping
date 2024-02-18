using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Core.Specs;
using Catalog.Infrastructure.Data;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories;

/// <summary>
/// Todo: 2.11.1 Repository Implementation
/// </summary>
public class ProductRepository : IProductRepository, IBrandRepository, ITypesRepository
{
    private readonly ICatalogContext _context;

    public ProductRepository(ICatalogContext context)
    {
        _context = context;
    }
    
    /// <summary>
    /// Todo: 2.11.1.1 Complex Query with Pagination
    /// </summary>
    /// <param name="catalogSpecParams"></param>
    /// <returns></returns>
    public async Task<Pagination<Product>> GetProducts(CatalogSpecParams catalogSpecParams)
    {
        var builder = Builders<Product>.Filter;
        var filter = builder.Empty;
        if(!string.IsNullOrEmpty(catalogSpecParams.Search))
        {
            var searchFilter = builder.Regex(x => x.Name, new BsonRegularExpression(catalogSpecParams.Search));
            filter &= searchFilter;
        }
        if(!string.IsNullOrEmpty(catalogSpecParams.BrandId))
        {
            var brandFilter = builder.Eq(x => x.Brands.Id,catalogSpecParams.BrandId);
            filter &= brandFilter;
        }
        if(!string.IsNullOrEmpty(catalogSpecParams.TypeId))
        {
            var typeFilter = builder.Eq(x => x.Types.Id, catalogSpecParams.TypeId);
            filter &= typeFilter;
        }

        if (!string.IsNullOrEmpty(catalogSpecParams.Sort))
        {
            return new Pagination<Product>
            {
                PageSize = catalogSpecParams.PageSize,
                PageIndex = catalogSpecParams.PageIndex,
                Data = await DataFilter(catalogSpecParams, filter),
                Count = await _context.Products.CountDocumentsAsync(p =>
                    true)
            };
        }

        return new Pagination<Product>
        {
            PageSize = catalogSpecParams.PageSize,
            PageIndex = catalogSpecParams.PageIndex,
            Data = await _context
                .Products
                .Find(filter)
                .Sort(Builders<Product>.Sort.Ascending("Name"))
                .Skip(catalogSpecParams.PageSize * (catalogSpecParams.PageIndex - 1))
                .Limit(catalogSpecParams.PageSize)
                .ToListAsync(),
            Count = await _context.Products.CountDocumentsAsync(p => true)
        };
    }

    private async Task<IReadOnlyList<Product>> DataFilter(CatalogSpecParams catalogSpecParams, FilterDefinition<Product> filter)
    {
        switch (catalogSpecParams.Sort)
        {
            case "priceAsc":
                return await _context
                    .Products
                    .Find(filter)
                    .Sort(Builders<Product>.Sort.Ascending("Price"))
                    .Skip(catalogSpecParams.PageSize * (catalogSpecParams.PageIndex - 1))
                    .Limit(catalogSpecParams.PageSize)
                    .ToListAsync();
            case "priceDesc":
                return await _context
                    .Products
                    .Find(filter)
                    .Sort(Builders<Product>.Sort.Descending("Price"))
                    .Skip(catalogSpecParams.PageSize * (catalogSpecParams.PageIndex - 1))
                    .Limit(catalogSpecParams.PageSize)
                    .ToListAsync();
            default:
                return await _context
                    .Products
                    .Find(filter)
                    .Sort(Builders<Product>.Sort.Ascending("Name"))
                    .Skip(catalogSpecParams.PageSize * (catalogSpecParams.PageIndex - 1))
                    .Limit(catalogSpecParams.PageSize)
                    .ToListAsync();
        }
    }

    /// <summary>
    /// Todo: 2.11.1.2 Query By Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<Product> GetProduct(string id)
    {
        return await _context
            .Products
            .Find(p => p.Id == id)
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// Todo: 2.11.1.3 Query By String
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public async Task<IEnumerable<Product>> GetProductByName(string name)
    {
        FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Name, name);
        return await _context
            .Products
            .Find(filter)
            .ToListAsync();

    }

    public async Task<IEnumerable<Product>> GetProductByBrand(string name)
    {
        FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Brands.Name, name);
        return await _context
            .Products
            .Find(filter)
            .ToListAsync();
    }

    /// <summary>
    /// Todo: 2.11.1.4 Insert 
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    public async Task<Product> CreateProduct(Product product)
    {
        await _context.Products.InsertOneAsync(product);
        return product;
    }
    
    /// <summary>
    /// Todo: 2.11.1.5 Update 
    /// updateResult.IsAcknowledged: Trả về một giá trị boolean cho biết liệu kết quả của thao tác cập nhật có được xác nhận hay không435.
    /// Nếu IsAcknowledged là true, điều này có nghĩa là thao tác cập nhật đã được MongoDB nhận và xử lý435.
    /// Nếu IsAcknowledged là false, thì sẽ ném ra một ngoại lệ3.
    ///
    /// updateResult.ModifiedCount: Trả về số lượng tài liệu đã được sửa đổi trong thao tác cập nhật26.
    /// Ví dụ, nếu bạn thực hiện một thao tác UpdateOneAsync và updateResult.ModifiedCount trả về 1, điều này có nghĩa là một tài liệu đã được cập nhật thành công2.
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    public async Task<bool> UpdateProduct(Product product)
    {
        var updateResult = await _context
            .Products
            .ReplaceOneAsync(p => p.Id == product.Id, product);
        return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
    }

    /// <summary>
    /// Todo: 2.11.1.6 Delete
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<bool> DeleteProduct(string id)
    {
        FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Id, id);
        DeleteResult deleteResult = await _context
            .Products
            .DeleteOneAsync(filter);
        return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
    }

    public async Task<IEnumerable<ProductBrand>> GetAllBrands()
    {
        return await _context
            .Brands
            .Find(b => true)
            .ToListAsync();
    }

    public async Task<IEnumerable<ProductType>> GetAllTypes()
    {
        return await _context
            .Types
            .Find(t => true)
            .ToListAsync();
    }
}