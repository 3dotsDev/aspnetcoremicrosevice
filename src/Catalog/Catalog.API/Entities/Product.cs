using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.API.Entities
{
    public class Product
    {
        [BsonId] // DB Type 
        [BsonRepresentation(BsonType.ObjectId)] //Kennzeichnet die Spalte als Kennung
        public string Id { get; set; }
        
        [BsonElement("Name")] //ColumnName auf der DB
        public string Name { get; set; }
        public string Category { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string ImageFile { get; set; }
        public decimal Price { get; set; }
    }
}