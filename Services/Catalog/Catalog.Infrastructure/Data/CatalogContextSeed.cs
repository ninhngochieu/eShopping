using System.Text.Json;
using Catalog.Core.Entities;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Data;

/// <summary>
/// Todo: 2.8.1 Seed mongo data 
/// </summary>
public class CatalogContextSeed
{
    public static void SeedData(IMongoCollection<Product> productCollection)
    {
#if DEBUG
        string path = Path.Combine("..",$"{nameof(Catalog)}.{nameof(Infrastructure)}","Data", "SeedData", "products.json");
#else
            string path = Path.Combine("Data", "SeedData", "products.json");
#endif
        bool checkProducts = productCollection.Find(b => true).Any();
        if (!checkProducts)
        {
            var productsData = File.ReadAllText(path);
            var products = JsonSerializer.Deserialize<List<Product>>(productsData);
            if (products != null)
            {
                foreach (var item in products)
                {
                    productCollection.InsertOneAsync(item);
                }
            }
        }
    }
}