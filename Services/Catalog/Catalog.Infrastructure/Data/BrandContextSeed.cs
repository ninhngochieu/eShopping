using System.Text.Json;
using Catalog.Core.Entities;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Data;

/// <summary>
/// Todo: 2.8.1 Seed mongo data
/// Seed data nếu chưa có
/// </summary>
public static class BrandContextSeed
{
    public static void SeedData(IMongoCollection<ProductBrand> brandCollection)
    {
#if DEBUG
        var path = Path.Combine("..", $"{nameof(Catalog)}.{nameof(Infrastructure)}", "Data", "SeedData", "brands.json");
#else
            string path = Path.Combine("Data", "SeedData", "brands.json");
#endif
        bool checkBrands = brandCollection.Find(b => true).Any();
        if (!checkBrands)
        {
            var brandsData = File.ReadAllText(path);
            var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
            if (brands != null)
            {
                foreach (var item in brands)
                {
                    brandCollection.InsertOneAsync(item);
                }
            }
        }
    } 
}