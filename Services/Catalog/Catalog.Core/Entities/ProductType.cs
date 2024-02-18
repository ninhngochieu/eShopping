using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.Core.Entities;

/// <summary>
/// Todo: 2.6.2 Create Sub-entity
/// </summary>
public class ProductType : BaseEntity
{
    /// <summary>
    /// Set name for Mongo Column
    /// </summary>
    [BsonElement("Name")]
    public string Name { get; set; }
}