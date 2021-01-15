using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.API.Data.Interfaces;
using Catalog.API.Entities;
using Catalog.API.Repositories.Interfaces;
using MongoDB.Driver;

namespace Catalog.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext _context;

        public ProductRepository(ICatalogContext catalogContext)
        {
            _context = catalogContext ?? throw new ArgumentException(nameof(catalogContext));
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context
                .Products
                .Find(p => true)
                .ToListAsync();
        }

        public async Task<Product> GetProduct(string id)
        {
            FilterDefinition<Product> filterPerId = Builders<Product>.Filter.Eq(p => p.Id, id);

            return await _context
                .Products
                .Find(filterPerId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            FilterDefinition<Product> filterPerName = Builders<Product>.Filter.ElemMatch(p => p.Name, name);

            return await _context
                .Products
                .Find(filterPerName)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByCategory(string category)
        {
            FilterDefinition<Product> filterPerCategory = Builders<Product>.Filter.ElemMatch(p => p.Category, category);

            return await _context
                .Products
                .Find(filterPerCategory)
                .ToListAsync();
        }

        public async Task Create(Product product)
        {
            await _context.Products.InsertOneAsync(product);
        }

        public async Task<bool> Update(Product product)
        {
            var updateResult = await _context
                .Products
                .ReplaceOneAsync(filter: f => f.Id == product.Id, replacement: product);
            return updateResult.IsAcknowledged
                   && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> Delete(string id)
        {
            FilterDefinition<Product> filterPerId = Builders<Product>.Filter.Eq(p => p.Id, id);
            DeleteResult deleteResult = await _context
                .Products
                .DeleteOneAsync(filterPerId);
            return deleteResult.IsAcknowledged
                   && deleteResult.DeletedCount > 0;
        }
    }
}