using Catalog.API.Data.Interfaces;
using Catalog.API.Entities;
using Catalog.API.Settings;
using MongoDB.Driver;

namespace Catalog.API.Data
{
    public class CatalogContext : ICatalogContext
    {
        private readonly ICatalogDatabaseSettings _settings;
        private IMongoDatabase _database;

        public CatalogContext(ICatalogDatabaseSettings settings)
        {
            _settings = settings;
            Products = SettingUpDatabase(settings);

            CatalogContextSeed.SeedDataIfEmptyCollection(Products);
        }

        private IMongoCollection<Product> SettingUpDatabase(ICatalogDatabaseSettings settings)
        {
            var client = new MongoClient(_settings.ConnectionString);
            _database = client.GetDatabase(settings.DatabaseName);
            return SettingUpDataCollection(settings);
        }

        private IMongoCollection<Product> SettingUpDataCollection(ICatalogDatabaseSettings settings)
        {
            return _database.GetCollection<Product>(_settings.CollectionName);
        }

        public IMongoCollection<Product> Products { get; }
    }
}