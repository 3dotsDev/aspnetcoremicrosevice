using System.Collections.Generic;
using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Data
{
    public class CatalogContextSeed
    {
        public static void SeedDataIfEmptyCollection(IMongoCollection<Product> products)
        {
            var existsProduct = products.Find(p => true).Any();
            if (!existsProduct)
            {
                products.InsertManyAsync(GetDummyProducts());
            }
        }

        private static IEnumerable<Product> GetDummyProducts()
        {
            return new List<Product>()
            {
                new Product()
                {
                    Name = "Iphone SE 2020",
                    Category = "Mobile Phones",
                    Summary = "Summary",
                    Description = "Description",
                    ImageFile = "Image.jpg",
                    Price = 99.00M
                },
                new Product()
                {
                    Name = "Iphone SE 2020",
                    Category = "Mobile Phones",
                    Summary = "Summary",
                    Description = "Description",
                    ImageFile = "Image.jpg",
                    Price = 99.00M
                },
                new Product()
                {
                    Name = "Iphone SE 2020",
                    Category = "Mobile Phones",
                    Summary = "Summary",
                    Description = "Description",
                    ImageFile = "Image.jpg",
                    Price = 99.00M
                },
                new Product()
                {
                    Name = "Iphone SE 2020",
                    Category = "Mobile Phones",
                    Summary = "Summary",
                    Description = "Description",
                    ImageFile = "Image.jpg",
                    Price = 99.00M
                },
                new Product()
                {
                    Name = "Iphone SE 2020",
                    Category = "Mobile Phones",
                    Summary = "Summary",
                    Description = "Description",
                    ImageFile = "Image.jpg",
                    Price = 99.00M
                },
                new Product()
                {
                    Name = "Iphone SE 2020",
                    Category = "Mobile Phones",
                    Summary = "Summary",
                    Description = "Description",
                    ImageFile = "Image.jpg",
                    Price = 99.00M
                },
                new Product()
                {
                    Name = "Iphone SE 2020",
                    Category = "Mobile Phones",
                    Summary = "Summary",
                    Description = "Description",
                    ImageFile = "Image.jpg",
                    Price = 99.00M
                },
                new Product()
                {
                    Name = "Iphone SE 2020",
                    Category = "Mobile Phones",
                    Summary = "Summary",
                    Description = "Description",
                    ImageFile = "Image.jpg",
                    Price = 99.00M
                },
                new Product()
                {
                    Name = "Iphone SE 2020",
                    Category = "Mobile Phones",
                    Summary = "Summary",
                    Description = "Description",
                    ImageFile = "Image.jpg",
                    Price = 99.00M
                }
            };
        }
    }
}