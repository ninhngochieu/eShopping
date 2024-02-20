using System.Text.Json;
using Catalog.Core.Entities;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Data;

public class TypeContextSeed
{
    public static void SeedData(IMongoCollection<ProductType> typeCollection)
    {
#if DEBUG
        string path = Path.Combine("..",$"{nameof(Catalog)}.{nameof(Infrastructure)}","Data", "SeedData", "types.json");
#else
            string path = Path.Combine("Data", "SeedData", "types.json");
#endif
        bool checkTypes = typeCollection.Find(b => true).Any();
        if (!checkTypes)
        {
            var typesData = File.ReadAllText(path);
            var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
            if (types != null)
            {
                foreach (var item in types)
                {
                    typeCollection.InsertOneAsync(item);
                }
            }
        }
    }
}