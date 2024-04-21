using Microsoft.EntityFrameworkCore;
using ProductApi.Domain.Entities;
using ProductApi.Persistence.DatabaseContexts;

namespace ProductApi.Tests.Helpers
{
    internal sealed class DatabaseContextBuilder
    {
        private readonly DatabaseContext _databaseContext;
        private readonly List<Product> _products;

        public DatabaseContextBuilder()
        {
            this._databaseContext = new DatabaseContext(new DbContextOptionsBuilder<DatabaseContext>().UseInMemoryDatabase("TestProductCatalog").Options);this._products = new List<Product>();
        }

        public DatabaseContext Build()
        {
            // delete database.
            this._databaseContext.Database.EnsureDeleted();

            // create fresh database.
            this._databaseContext.Database.EnsureCreated();
            this._databaseContext.Products.AddRange(this._products);

            this._databaseContext.SaveChanges();
            return this._databaseContext;
        }

        public DatabaseContextBuilder WithProduct(int? id, string name, string brandName, decimal price)
        {
            var product = new Product
            {
                Id = id.HasValue ? id.Value : default(int),
                Name = name,
                BrandName = brandName,
                Price = price
            };

            this._products.Add(product);
            return this;
        }
    }
}