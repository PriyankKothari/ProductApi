using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using ProductApi.Domain.Entities;
using ProductApi.Domain.Repositories;
using ProductApi.Persistence.Repositories;
using ProductApi.Tests.Helpers;
using System.Linq.Expressions;

namespace ProductApi.Tests.Repositories
{
    [TestClass]
    public class ProductRepositoryTests
    {
        private Mock<ILogger<IProductRepository>> _logger = new();

        [TestMethod]
        public async Task ListAsync_ShouldReturnNoProduct_WhenNoProductsExist()
        {
            // Arrange
            IProductRepository productRepository = new ProductRepository(new DatabaseContextBuilder().Build(), this._logger.Object);

            // Act
            var result = await productRepository.ListAsync(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<CancellationToken>());

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public async Task ListAsync_ShouldReturnAllProducts_WhenProductsExist()
        {
            // Arrange
            IProductRepository productRepository = new ProductRepository(
                new DatabaseContextBuilder()
                .WithProduct(1, "Product One", "Brand One", 10.00m)
                .WithProduct(2, "Product Two", "Brand Two", 20.50m).Build(),
                this._logger.Object);

            // Act
            var result = await productRepository.ListAsync(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<CancellationToken>());

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public async Task ListAsync_ShouldReturnAllMatchingProducts_When_ProductsWithBrandNameExists()
        {
            // Arrange
            IProductRepository productRepository = new ProductRepository(
                new DatabaseContextBuilder()
                .WithProduct(1, "Product One", "Brand One", 10.00m)
                .WithProduct(2, "Product Two", "Brand Two", 20.50m)
                .WithProduct(3, "Product Three", "Brand Two", 25.00m).Build(),
                this._logger.Object);

            // Act
            var result = await productRepository.ListAsync(pr => pr.BrandName == "Brand Two", It.IsAny<CancellationToken>());

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public async Task GetAsync_ShouldReturnNullProduct_WhenProductWithMatchingNameDoesNotExist()
        {
            // Arrange
            Product product = new() { Id = 4, Name = "Monitor", BrandName = "Dell", Price = 400 };

            IProductRepository productRepository = new ProductRepository(
                new DatabaseContextBuilder()
                .WithProduct(1, "Laptop", "Dell", 1000.00m)
                .WithProduct(2, "Docking Station", "HP", 255.70m)
                .WithProduct(3, "Hard Drive", "Intel", 375.00m)
                .WithProduct(product.Id, product.Name, product.BrandName, product.Price).Build(),
                this._logger.Object);

            // Act
            var result = await productRepository.GetAsync(pr => pr.Name == "Keyboard", It.IsAny<CancellationToken>());

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetAsync_ShouldReturnAProduct_WhenProductWithMatchingNameExists()
        {
            // Arrange
            Product product = new() { Id = 4, Name = "Monitor", BrandName = "Dell", Price = 400 };

            IProductRepository productRepository = new ProductRepository(
                new DatabaseContextBuilder()
                .WithProduct(1, "Laptop", "Dell", 1000.00m)
                .WithProduct(2, "Docking Station", "HP", 255.70m)
                .WithProduct(3, "Hard Drive", "Intel", 375.00m)
                .WithProduct(product.Id, product.Name, product.BrandName, product.Price).Build(),
                this._logger.Object);

            // Act
            var result = await productRepository.GetAsync(pr => pr.Name == "Monitor", It.IsAny<CancellationToken>());

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(product.Id, result.Id);
            Assert.AreEqual(product.Name, result.Name);
            Assert.AreEqual(product.BrandName, result.BrandName);
            Assert.AreEqual(product.Price, result.Price);
        }

        [TestMethod]
        public async Task GetAsync_ShouldReturnAProduct_WhenProductWithMatchingNameAndBrandExists()
        {
            // Arrange
            Product product = new() { Id = 4, Name = "Monitor", BrandName = "Dell", Price = 400 };

            IProductRepository productRepository = new ProductRepository(
                new DatabaseContextBuilder()
                .WithProduct(1, "Laptop", "Dell", 1000.00m)
                .WithProduct(2, "Docking Station", "HP", 255.70m)
                .WithProduct(3, "Hard Drive", "Intel", 375.00m)
                .WithProduct(product.Id, product.Name, product.BrandName, product.Price).Build(),
                this._logger.Object);

            // Act
            var result = await productRepository.GetAsync(pr => pr.Name == "Monitor" && pr.BrandName == "Dell", It.IsAny<CancellationToken>());

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(product.Id, result.Id);
            Assert.AreEqual(product.Name, result.Name);
            Assert.AreEqual(product.BrandName, result.BrandName);
            Assert.AreEqual(product.Price, result.Price);
        }

        [TestMethod]
        public async Task CreateAsync_ShouldCreateAProduct_WhenProductIsValid()
        {
            // Arrange
            var id = 1;
            var productName = "Product Name";
            var brandName = "Brand Name";
            var price = 125.75m;

            IProductRepository productRepository = new ProductRepository(new DatabaseContextBuilder().Build(), this._logger.Object);

            // Act
            var result = await productRepository.CreateAsync(new Product { Id = id, Name = productName, BrandName = brandName, Price = price }, It.IsAny<CancellationToken>());

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(id, result.Id);
            Assert.AreEqual(productName, result.Name);
            Assert.AreEqual(brandName, result.BrandName);
            Assert.AreEqual(price, result.Price);
        }

        [TestMethod]
        public async Task UpdateAsync_ShouldThrowDbUpdateConcurrencyException_WhenProductIsNotFoundForUpdate()
        {
            // Arrange
            var id = 1;
            var productName = "Product Name";
            var brandName = "Brand Name";
            var price = 125.75m;

            var productNameToUpdate = "My Special Product";
            var brandNameToUpdate = "Family Brand";
            var priceToUpdate = 100.00m;

            IProductRepository productRepository = new ProductRepository(
                new DatabaseContextBuilder()
                .WithProduct(id, productName, brandName, price)
                .Build(),
                this._logger.Object);

            // Act
            
            // Assert
            await Assert.ThrowsExceptionAsync<DbUpdateConcurrencyException>(async () =>
            {
                await
                productRepository.UpdateAsync(
                    new Product
                    {
                        Id = 2,
                        Name = productNameToUpdate,
                        BrandName = brandNameToUpdate,
                        Price = priceToUpdate
                    },
                    It.IsAny<CancellationToken>()).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task UpdateAsync_ShouldUpdateAProduct_WhenProductIsFoundForUpdate()
        {
            // Arrange
            var id = 1;
            var productName = "Product Name";
            var brandName = "Brand Name";
            var price = 125.75m;

            var productNameToUpdate = "My Special Product";
            var brandNameToUpdate = "Family Brand";
            var priceToUpdate = 100.00m;

            IProductRepository productRepository = new ProductRepository(
                new DatabaseContextBuilder()
                .WithProduct(id, productName, brandName, price)
                .Build(),
                this._logger.Object);

            // Act
            var result = await productRepository.UpdateAsync(new Product { Id = id, Name = productNameToUpdate, BrandName = brandNameToUpdate, Price = priceToUpdate }, It.IsAny<CancellationToken>());

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(id, result.Id);
            Assert.AreEqual(productNameToUpdate, result.Name);
            Assert.AreEqual(brandNameToUpdate, result.BrandName);
            Assert.AreEqual(priceToUpdate, result.Price);
        }

        [TestMethod]
        public async Task DeleteAsync_ShouldThrowDbUpdateConcurrencyException_WhenProductIsNotFoundForDelete()
        {
            // Arrange
            var id = 1;
            var productName = "Product Name";
            var brandName = "Brand Name";
            var price = 125.75m;

            var productNameToUpdate = "My Special Product";
            var brandNameToUpdate = "Family Brand";
            var priceToUpdate = 100.00m;

            IProductRepository productRepository = new ProductRepository(
                new DatabaseContextBuilder()
                .WithProduct(id, productName, brandName, price)
                .Build(),
                this._logger.Object);

            // Act

            // Assert
            await Assert.ThrowsExceptionAsync<DbUpdateConcurrencyException>(async () =>
            {
                await
                productRepository.DeleteAsync(
                    new Product
                    {
                        Id = 2,
                        Name = productNameToUpdate,
                        BrandName = brandNameToUpdate,
                        Price = priceToUpdate
                    },
                    It.IsAny<CancellationToken>()).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task DeleteAsync_ShouldDeleteAProduct_WhenProductIsFoundForDelete()
        {
            // Arrange
            var id = 1;
            var productName = "Product Name";
            var brandName = "Brand Name";
            var price = 125.75m;

            IProductRepository productRepository = new ProductRepository(
                new DatabaseContextBuilder()
                .WithProduct(id, productName, brandName, price)
                .Build(),
                this._logger.Object);

            // Act
            var result = await productRepository.DeleteAsync(new Product { Id = id, Name = productName, BrandName = brandName, Price = price }, It.IsAny<CancellationToken>());

            // Assert
            Assert.IsTrue(result);

            var products = await productRepository.ListAsync(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<CancellationToken>()).ConfigureAwait(false);
            Assert.IsFalse(products.Any());
        }
    }
}