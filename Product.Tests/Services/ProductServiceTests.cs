using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using ProductApi.Application.DTOs;
using ProductApi.Application.Services;
using ProductApi.Domain.Entities;
using ProductApi.Domain.Repositories;
using System.Linq.Expressions;

namespace ProductApi.Tests.Services
{
    [TestClass]
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _productRepository;
        private readonly Mock<ILogger<IProductService>> _logger;
        private readonly Mock<IMapper> _mapper;

        public ProductServiceTests()
        {
            this._productRepository = new();
            this._logger = new();
            this._mapper = new();
        }

        [TestMethod]
        public void ProductService_ThrowsNullException_When_ProductRepositoryIsNull()
        {
            // Arrange

            // Act

            // Assert
            Assert.ThrowsException<ArgumentNullException>(() => new ProductService(It.IsAny<IProductRepository>(), _logger.Object, _mapper.Object));
        }

        [TestMethod]
        public void ProductService_ThrowsNullException_When_LoggerIsNull()
        {
            // Arrange

            // Act

            // Assert
            Assert.ThrowsException<ArgumentNullException>(() => new ProductService(new Mock<IProductRepository>().Object, It.IsAny<ILogger<IProductService>>(), _mapper.Object));
        }

        [TestMethod]
        public void ProductService_ThrowsNullException_When_MapperIsNull()
        {
            // Arrange

            // Act

            // Assert
            Assert.ThrowsException<ArgumentNullException>(() => new ProductService(new Mock<IProductRepository>().Object, _logger.Object, It.IsAny<IMapper>()));
        }

        [TestMethod]
        public async Task ListProductsAsync_ShouldReturnAllProducts()
        {
            // Arrange
            List<Product> products = new List<Product>
            {
                new() { Id = 1, Name = "Product Name One", BrandName = "Brand Name One", Price = 10.00m },
                new() { Id = 2, Name = "Product Name Two", BrandName = "Brand Name One", Price = 10.00m },
                new() { Id = 3, Name = "Product Name Three", BrandName = "Brand Name Two", Price = 10.00m },
                new() { Id = 4, Name = "Product Name Four", BrandName = "Brand Name Three", Price = 10.00m },
                new() { Id = 5, Name = "Product Name Five", BrandName = "Brand Name Three", Price = 10.00m }
            };

            this._productRepository
                .Setup(pr => pr.ListAsync(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(products);

            this._mapper
                .Setup(m => m.Map<IEnumerable<ProductDto>>(products))
                .Returns(new List<ProductDto> 
                {
                    new() { Id = 1, Name = "Product Name One", BrandName = "Brand Name One", Price = 10.00m },
                    new() { Id = 2, Name = "Product Name Two", BrandName = "Brand Name One", Price = 10.00m },
                    new() { Id = 3, Name = "Product Name Three", BrandName = "Brand Name Two", Price = 10.00m },
                    new() { Id = 4, Name = "Product Name Four", BrandName = "Brand Name Three", Price = 10.00m },
                    new() { Id = 5, Name = "Product Name Five", BrandName = "Brand Name Three", Price = 10.00m }
                });

            IProductService productService = new ProductService(_productRepository.Object, _logger.Object, _mapper.Object);

            // Act
            var result = await productService.ListProductsAsync(It.IsAny<CancellationToken>()).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.Count());
        }

        [TestMethod]
        public async Task ListProductsByBrandNameAsync_ShouldThrowException_WhenBrandNameIsNullOrEmpty()
        {
            // Arrange
            IProductService productService = new ProductService(_productRepository.Object, _logger.Object, _mapper.Object);

            // Act            

            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(
                async() =>
                    await productService.ListProductsByBrandNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()).ConfigureAwait(false))
                .ConfigureAwait(false);
        }

        [TestMethod]
        public async Task ListProductsByBrandNameAsync_ShouldReturnOnlyProductsWithMatchingName_WhenNoMatchingBrandNameIsProvided()
        {
            // Arrange
            List<Product> products = new List<Product>
            {
                new() { Id = 1, Name = "Product Name One", BrandName = "Brand Name One", Price = 10.00m },
                new() { Id = 2, Name = "Product Name Two", BrandName = "Brand Name One", Price = 10.00m },
                new() { Id = 3, Name = "Product Name Three", BrandName = "Brand Name Two", Price = 10.00m },
                new() { Id = 4, Name = "Product Name Four", BrandName = "Brand Name Three", Price = 10.00m },
                new() { Id = 5, Name = "Product Name Five", BrandName = "Brand Name Three", Price = 10.00m }
            };

            this._productRepository
                .Setup(pr => pr.ListAsync(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Product>());

            this._mapper
                .Setup(m => m.Map<IEnumerable<ProductDto>>(It.IsAny<IEnumerable<Product>>()))
                .Returns(new List<ProductDto>());

            IProductService productService = new ProductService(_productRepository.Object, _logger.Object, _mapper.Object);

            // Act
            var result = await productService.ListProductsByBrandNameAsync("Brand Name Zero", It.IsAny<CancellationToken>()).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public async Task ListProductsByBrandNameAsync_ShouldReturnOnlyProductsWithMatchingName_WhenMatchingBrandNameIsProvided()
        {
            // Arrange
            List<Product> products = new List<Product>
            {
                new() { Id = 1, Name = "Product Name One", BrandName = "Brand Name One", Price = 10.00m },
                new() { Id = 2, Name = "Product Name Two", BrandName = "Brand Name One", Price = 10.00m },
                new() { Id = 3, Name = "Product Name Three", BrandName = "Brand Name Two", Price = 10.00m },
                new() { Id = 4, Name = "Product Name Four", BrandName = "Brand Name Three", Price = 10.00m },
                new() { Id = 5, Name = "Product Name Five", BrandName = "Brand Name Three", Price = 10.00m }
            };

            this._productRepository
                .Setup(pr => pr.ListAsync(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Product>
                {
                    new() { Id = 4, Name = "Product Name Four", BrandName = "Brand Name Three", Price = 10.00m },
                    new() { Id = 5, Name = "Product Name Five", BrandName = "Brand Name Three", Price = 10.00m }
                });

            this._mapper
                .Setup(m => m.Map<IEnumerable<ProductDto>>(It.IsAny<IEnumerable<Product>>()))
                .Returns(new List<ProductDto>
                {
                    new() { Id = 4, Name = "Product Name Four", BrandName = "Brand Name Three", Price = 10.00m },
                    new() { Id = 5, Name = "Product Name Five", BrandName = "Brand Name Three", Price = 10.00m }
                });

            IProductService productService = new ProductService(_productRepository.Object, _logger.Object, _mapper.Object);

            // Act
            var result = await productService.ListProductsByBrandNameAsync("Brand Name Three", It.IsAny<CancellationToken>()).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public async Task GetProductByIdAsync_ShouldThrowException_WhenIdIsNullOrEmpty()
        {
            // Arrange
            IProductService productService = new ProductService(_productRepository.Object, _logger.Object, _mapper.Object);

            // Act            

            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(
                async () =>
                    await productService.GetProductByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()).ConfigureAwait(false))
                .ConfigureAwait(false);
        }

        [TestMethod]
        public async Task GetProductByIdAsync_ShouldReturnNull_WhenNoProductIsFound()
        {
            // Arrange
            Product product = new() { Id = 1, Name = "Product Name One", BrandName = "Brand Name One", Price = 10.00m };

            this._productRepository
                .Setup(pr => pr.GetAsync(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            this._mapper
                .Setup(m => m.Map<ProductDto>(It.IsAny<Product>()))
                .Returns(It.IsAny<ProductDto>());

            IProductService productService = new ProductService(_productRepository.Object, _logger.Object, _mapper.Object);

            // Act
            var result = await productService.GetProductByIdAsync(1, It.IsAny<CancellationToken>()).ConfigureAwait(false);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetProductByIdAsync_ShouldReturnProduct_WhenProductIsFound()
        {
            // Arrange
            Product product = new() { Id = 1, Name = "Product Name One", BrandName = "Brand Name One", Price = 10.00m };

            this._productRepository
                .Setup(pr => pr.GetAsync(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            this._mapper
                .Setup(m => m.Map<ProductDto>(It.IsAny<Product>()))
                .Returns(new ProductDto { Id = product.Id, Name = product.Name, BrandName = product.BrandName, Price = product.Price });

            IProductService productService = new ProductService(_productRepository.Object, _logger.Object, _mapper.Object);

            // Act
            var result = await productService.GetProductByIdAsync(1, It.IsAny<CancellationToken>()).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Id, product.Id);
            Assert.AreEqual(result.Name, product.Name);
            Assert.AreEqual(result.BrandName, product.BrandName);
            Assert.AreEqual(result.Price, product.Price);
        }

        [TestMethod]
        public async Task GetProductByNameAsync_ShouldThrowException_WhenNameIsNullOrEmpty()
        {
            // Arrange
            IProductService productService = new ProductService(_productRepository.Object, _logger.Object, _mapper.Object);

            // Act            

            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(
                async () =>
                    await productService.GetProductByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()).ConfigureAwait(false))
                .ConfigureAwait(false);
        }

        [TestMethod]
        public async Task GetProductByNameAsync_ShouldReturnNull_WhenNoProductIsFound()
        {
            // Arrange
            Product product = new() { Id = 1, Name = "Product Name One", BrandName = "Brand Name One", Price = 10.00m };

            this._productRepository
                .Setup(pr => pr.GetAsync(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            this._mapper
                .Setup(m => m.Map<ProductDto>(It.IsAny<Product>()))
                .Returns(It.IsAny<ProductDto>());

            IProductService productService = new ProductService(_productRepository.Object, _logger.Object, _mapper.Object);

            // Act
            var result = await productService.GetProductByNameAsync("Product Name", It.IsAny<CancellationToken>()).ConfigureAwait(false);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetProductByNameAsync_ShouldReturnProduct_WhenProductIsFound()
        {
            // Arrange
            Product product = new() { Id = 1, Name = "Product Name One", BrandName = "Brand Name One", Price = 10.00m };

            this._productRepository
                .Setup(pr => pr.GetAsync(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            this._mapper
                .Setup(m => m.Map<ProductDto>(It.IsAny<Product>()))
                .Returns(new ProductDto { Id = product.Id, Name = product.Name, BrandName = product.BrandName, Price = product.Price });

            IProductService productService = new ProductService(_productRepository.Object, _logger.Object, _mapper.Object);

            // Act
            var result = await productService.GetProductByNameAsync("Product Name One", It.IsAny<CancellationToken>()).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Id, product.Id);
            Assert.AreEqual(result.Name, product.Name);
            Assert.AreEqual(result.BrandName, product.BrandName);
            Assert.AreEqual(result.Price, product.Price);
        }

        [TestMethod]
        public async Task GetProductByNameAndBrandAsync_ShouldThrowException_WhenNameIsNullOrEmpty()
        {
            // Arrange
            IProductService productService = new ProductService(_productRepository.Object, _logger.Object, _mapper.Object);

            // Act            

            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(
                async () =>
                    await productService.GetProductByNameAndBrandAsync(It.IsAny<string>(), "Brand Name One", It.IsAny<CancellationToken>()).ConfigureAwait(false))
                .ConfigureAwait(false);
        }

        [TestMethod]
        public async Task GetProductByNameAndBrandAsync_ShouldThrowException_WhenBrandNameIsNullOrEmpty()
        {
            // Arrange
            IProductService productService = new ProductService(_productRepository.Object, _logger.Object, _mapper.Object);

            // Act            

            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(
                async () =>
                    await productService.GetProductByNameAndBrandAsync("Product Name One", It.IsAny<string>(), It.IsAny<CancellationToken>()).ConfigureAwait(false))
                .ConfigureAwait(false);
        }

        [TestMethod]
        public async Task GetProductByNameAndBrandAsync_ShouldReturnNull_WhenNameIsNotMatchingAndNoProductIsFound()
        {
            // Arrange
            Product product = new() { Id = 1, Name = "Product Name One", BrandName = "Brand Name One", Price = 10.00m };

            this._productRepository
                .Setup(pr => pr.GetAsync(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            this._mapper
                .Setup(m => m.Map<ProductDto>(It.IsAny<Product>()))
                .Returns(It.IsAny<ProductDto>());

            IProductService productService = new ProductService(_productRepository.Object, _logger.Object, _mapper.Object);

            // Act
            var result = await productService.GetProductByNameAndBrandAsync("Product Name", "Brand Name One", It.IsAny<CancellationToken>()).ConfigureAwait(false);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetProductByNameAndBrandAsync_ShouldReturnNull_WhenBrandNameIsNotMatchingAndNoProductIsFound()
        {
            // Arrange
            Product product = new() { Id = 1, Name = "Product Name One", BrandName = "Brand Name One", Price = 10.00m };

            this._productRepository
                .Setup(pr => pr.GetAsync(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            this._mapper
                .Setup(m => m.Map<ProductDto>(It.IsAny<Product>()))
                .Returns(It.IsAny<ProductDto>());

            IProductService productService = new ProductService(_productRepository.Object, _logger.Object, _mapper.Object);

            // Act
            var result = await productService.GetProductByNameAndBrandAsync("Product Name One", "Brand Name", It.IsAny<CancellationToken>()).ConfigureAwait(false);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetProductByNameAndBrandAsync_ShouldReturnProduct_WhenNameAndBrandNameIsMatchingAndProductIsFound()
        {
            // Arrange
            Product product = new() { Id = 1, Name = "Product Name One", BrandName = "Brand Name One", Price = 10.00m };

            this._productRepository
                .Setup(pr => pr.GetAsync(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            this._mapper
                .Setup(m => m.Map<ProductDto>(It.IsAny<Product>()))
                .Returns(new ProductDto { Id = product.Id, Name = product.Name, BrandName = product.BrandName, Price = product.Price });

            IProductService productService = new ProductService(_productRepository.Object, _logger.Object, _mapper.Object);

            // Act
            var result = await productService.GetProductByNameAndBrandAsync("Product Name One", "Brand Name One", It.IsAny<CancellationToken>()).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Id, product.Id);
            Assert.AreEqual(result.Name, product.Name);
            Assert.AreEqual(result.BrandName, product.BrandName);
            Assert.AreEqual(result.Price, product.Price);
        }

        [TestMethod]
        public async Task CreateProductAsync_ShouldThrowException_WhenProductRequestIsNull()
        {
            // Arrange
            IProductService productService = new ProductService(_productRepository.Object, _logger.Object, _mapper.Object);

            // Act

            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(
                async () =>
                    await productService.CreateProductAsync(It.IsAny<ProductDto>(), It.IsAny<CancellationToken>()).ConfigureAwait(false))
                .ConfigureAwait(false);
        }

        [TestMethod]
        public async Task CreateProductAsync_ShouldCreateProduct_WhenProductRequestIsProvided()
        {
            // Arrange
            Product product = new() { Id = 1, Name = "Product Name One", BrandName = "Brand Name One", Price = 10.00m };

            this._productRepository
                .Setup(pr => pr.CreateAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            this._mapper
                .Setup(m => m.Map<ProductDto>(It.IsAny<Product>()))
                .Returns(new ProductDto { Id = product.Id, Name = product.Name, BrandName = product.BrandName, Price = product.Price });

            IProductService productService = new ProductService(_productRepository.Object, _logger.Object, _mapper.Object);

            // Act
            var result = await productService.CreateProductAsync(
                new ProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    BrandName = product.BrandName,
                    Price = product.Price
                }, It.IsAny<CancellationToken>()).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Id, product.Id);
            Assert.AreEqual(result.Name, product.Name);
            Assert.AreEqual(result.BrandName, product.BrandName);
            Assert.AreEqual(result.Price, product.Price);
        }

        [TestMethod]
        public async Task UpdateProductAsync_ShouldThrowException_WhenProductRequestIsNull()
        {
            // Arrange
            IProductService productService = new ProductService(_productRepository.Object, _logger.Object, _mapper.Object);

            // Act

            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(
                async () =>
                    await productService.UpdateProductAsync(It.IsAny<ProductDto>(), It.IsAny<CancellationToken>()).ConfigureAwait(false))
                .ConfigureAwait(false);
        }

        [TestMethod]
        public async Task UpdateProductAsync_ShouldUpdateProduct_WhenProductRequestIsProvided()
        {
            // Arrange
            Product product = new() { Id = 1, Name = "Product Name One", BrandName = "Brand Name One", Price = 10.00m };

            this._productRepository
                .Setup(pr => pr.UpdateAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            this._mapper
                .Setup(m => m.Map<ProductDto>(It.IsAny<Product>()))
                .Returns(new ProductDto { Id = product.Id, Name = product.Name, BrandName = product.BrandName, Price = product.Price });

            IProductService productService = new ProductService(_productRepository.Object, _logger.Object, _mapper.Object);

            // Act
            var result = await productService.UpdateProductAsync(
                new ProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    BrandName = product.BrandName,
                    Price = product.Price
                }, It.IsAny<CancellationToken>()).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Id, product.Id);
            Assert.AreEqual(result.Name, product.Name);
            Assert.AreEqual(result.BrandName, product.BrandName);
            Assert.AreEqual(result.Price, product.Price);
        }

        [TestMethod]
        public async Task DeleteProductAsync_ShouldThrowException_WhenProductRequestIsNull()
        {
            // Arrange
            IProductService productService = new ProductService(_productRepository.Object, _logger.Object, _mapper.Object);

            // Act

            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(
                async () =>
                    await productService.DeleteProductAsync(It.IsAny<ProductDto>(), It.IsAny<CancellationToken>()).ConfigureAwait(false))
                .ConfigureAwait(false);
        }

        [TestMethod]
        public async Task DeleteProductAsync_ShouldDeleteProduct_WhenProductRequestIsProvided()
        {
            // Arrange
            Product product = new() { Id = 1, Name = "Product Name One", BrandName = "Brand Name One", Price = 10.00m };

            this._productRepository
                .Setup(pr => pr.DeleteAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            IProductService productService = new ProductService(_productRepository.Object, _logger.Object, _mapper.Object);

            // Act
            var result = await productService.DeleteProductAsync(
                new ProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    BrandName = product.BrandName,
                    Price = product.Price
                }, It.IsAny<CancellationToken>()).ConfigureAwait(false);

            // Assert
            Assert.IsTrue(result);
        }
    }
}