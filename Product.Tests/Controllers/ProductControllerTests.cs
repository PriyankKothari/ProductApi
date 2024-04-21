using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ProductApi.Application.Services;
using ProductApi.WebApi.Controllers;
using ProductApi.WebApi.ViewModels;
using System.Net;
using ProductApi.Application.DTOs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ProductApi.Tests.Controllers
{
    [TestClass]
    public class ProductControllerTests
    {
        private Mock<IProductService> _productService;
        private readonly Mock<ILogger<ProductsController>> _logger;
        private readonly Mock<IMapper> _mapper;
        
        private ProductsController _productsController;

        public ProductControllerTests()
        {
            this._productService = new();
            this._mapper = new();
            this._logger = new();
        }

        [TestMethod]
        public void ProductsController_ThrowsNullException_When_ProductServiceIsNull()
        {
            // Arrange

            // Act

            // Assert
            Assert.ThrowsException<ArgumentNullException>(() => new ProductsController(It.IsAny<IProductService>(), _logger.Object, _mapper.Object));
        }

        [TestMethod]
        public void ProductsController_ThrowsNullException_When_LoggerIsNull()
        {
            // Arrange

            // Act

            // Assert
            Assert.ThrowsException<ArgumentNullException>(() => new ProductsController(new Mock<IProductService>().Object, It.IsAny<ILogger<ProductsController>>(), _mapper.Object));
        }

        [TestMethod]
        public void ProductsController_ThrowsNullException_When_MapperIsNull()
        {
            // Arrange

            // Act

            // Assert
            Assert.ThrowsException<ArgumentNullException>(() => new ProductsController(new Mock<IProductService>().Object, _logger.Object, It.IsAny<IMapper>()));
        }

        [TestMethod]
        public async Task Get_ShouldReturnJsonResult_WithOkHttpStatusCode()
        {
            // Arrange
            this._productService
                .Setup(ps => ps.ListProductsAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ProductDto>());

            this._productsController = new ProductsController(this._productService.Object, this._logger.Object, this._mapper.Object);

            // Act
            var jsonResult = await this._productsController.Get(It.IsAny<CancellationToken>()) as JsonResult;

            // Assert
            Assert.IsNotNull(jsonResult);
            Assert.AreEqual((int)HttpStatusCode.OK, jsonResult.StatusCode);
        }

        [TestMethod]
        public async Task GetById_ShouldReturnErrorMessage_WithNotFoundHttpStatusCode()
        {
            // Arrange
            this._productService
                .Setup(ps => ps.GetProductByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(It.IsAny<ProductDto>());

            this._productsController = new ProductsController(this._productService.Object, this._logger.Object, this._mapper.Object);

            // Act
            var jsonResult = await this._productsController.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>()).ConfigureAwait(false) as JsonResult;

            // Assert
            Assert.IsNotNull(jsonResult);
            Assert.AreEqual((int)HttpStatusCode.NotFound, jsonResult.StatusCode);
            Assert.AreEqual($"We can't find any product by Id '{It.IsAny<int>()}'", JObject.Parse(JsonConvert.SerializeObject(jsonResult.Value))["Errors"]?.FirstOrDefault());
        }

        [TestMethod]
        public async Task GetById_ShouldReturnJsonResult_WithOkHttpStatusCode()
        {
            // Arrange
            ProductDto productDto = new ProductDto { Id = 1, Name = "Product Name", BrandName = "Brand Name", Price = 10.00m };

            this._productService
                .Setup(ps => ps.GetProductByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(productDto);

            this._mapper
                .Setup(m => m.Map<ProductResponse>(It.IsAny<ProductDto>()))
                .Returns(new ProductResponse { Id = productDto.Id, Name = productDto.Name, BrandName = productDto.BrandName, Price = productDto.Price });

            this._productsController = new ProductsController(this._productService.Object, this._logger.Object, this._mapper.Object);

            // Act
            var jsonResult = await this._productsController.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>()).ConfigureAwait(false) as JsonResult;

            // Assert
            Assert.IsNotNull(jsonResult);
            Assert.AreEqual((int)HttpStatusCode.OK, jsonResult.StatusCode);

            var product = JObject.Parse(JsonConvert.SerializeObject(jsonResult.Value))["Product"];
            Assert.IsNotNull(product);
            Assert.AreEqual(product["Id"]?.Value<int>(), productDto.Id);
            Assert.AreEqual(product["Name"]?.Value<string>(), productDto.Name);
            Assert.AreEqual(product["BrandName"]?.Value<string>(), productDto.BrandName);
            Assert.AreEqual(product["Price"]?.Value<decimal>(), productDto.Price);
        }

        [TestMethod]
        public async Task GetByName_ShouldReturnErrorMessage_WithNotFoundHttpStatusCode()
        {
            // Arrange
            string name = "product";

            this._productService
                .Setup(ps => ps.GetProductByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(It.IsAny<ProductDto>());

            this._productsController = new ProductsController(this._productService.Object, this._logger.Object, this._mapper.Object);

            // Act
            var jsonResult = await this._productsController.GetByName(name, It.IsAny<CancellationToken>()).ConfigureAwait(false) as JsonResult;

            // Assert
            Assert.IsNotNull(jsonResult);
            Assert.AreEqual((int)HttpStatusCode.NotFound, jsonResult.StatusCode);
            Assert.AreEqual($"We can't find any product by Name '{name}'", JObject.Parse(JsonConvert.SerializeObject(jsonResult.Value))["Errors"]?.FirstOrDefault());
        }

        [TestMethod]
        public async Task GetByName_ShouldReturnJsonResult_WithOkHttpStatusCode()
        {
            // Arrange
            ProductDto productDto = new ProductDto { Id = 1, Name = "Product One", BrandName = "Brand One", Price = 30.00m };

            this._productService
                .Setup(ps => ps.GetProductByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(productDto);

            this._mapper
                .Setup(m => m.Map<ProductResponse>(It.IsAny<ProductDto>()))
                .Returns(new ProductResponse { Id = productDto.Id, Name = productDto.Name, BrandName = productDto.BrandName, Price = productDto.Price });

            this._productsController = new ProductsController(this._productService.Object, this._logger.Object, this._mapper.Object);

            // Act
            var jsonResult = await this._productsController.GetByName(productDto.Name, It.IsAny<CancellationToken>()).ConfigureAwait(false) as JsonResult;

            // Assert
            Assert.IsNotNull(jsonResult);
            Assert.AreEqual((int)HttpStatusCode.OK, jsonResult.StatusCode);

            var product = JObject.Parse(JsonConvert.SerializeObject(jsonResult.Value))["Product"];
            Assert.IsNotNull(product);
            Assert.AreEqual(product["Id"]?.Value<int>(), productDto.Id);
            Assert.AreEqual(product["Name"]?.Value<string>(), productDto.Name);
            Assert.AreEqual(product["BrandName"]?.Value<string>(), productDto.BrandName);
            Assert.AreEqual(product["Price"]?.Value<decimal>(), productDto.Price);
        }

        [TestMethod]
        public async Task GetByBrandName_Should_ReturnJsonResult_WithOkHttpStatusCode()
        {
            // Arrange
            this._productService
                .Setup(ps => ps.ListProductsByBrandNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ProductDto>());

            this._productsController = new ProductsController(this._productService.Object, this._logger.Object, this._mapper.Object);

            // Act
            var jsonResult = await this._productsController.GetByBrandName("Brand Name", It.IsAny<CancellationToken>()) as JsonResult;

            // Assert
            Assert.IsNotNull(jsonResult);
            Assert.AreEqual((int)HttpStatusCode.OK, jsonResult.StatusCode);
        }

        [TestMethod]
        public async Task GetByNameAndBrandName_ShouldReturnErrorMessage_WithNotFoundHttpStatusCode()
        {
            // Arrange
            string name = "product";
            string brandName = "brand";

            this._productService
                .Setup(ps => ps.GetProductByNameAndBrandAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(It.IsAny<ProductDto>());

            this._productsController = new ProductsController(this._productService.Object, this._logger.Object, this._mapper.Object);

            // Act
            var jsonResult = await this._productsController.GetByNameAndBrandName(name, brandName, It.IsAny<CancellationToken>()).ConfigureAwait(false) as JsonResult;

            // Assert
            Assert.IsNotNull(jsonResult);
            Assert.AreEqual((int)HttpStatusCode.NotFound, jsonResult.StatusCode);
            Assert.AreEqual($"We can't find the product by Name '{name}' and Brand Name '{brandName}'", JObject.Parse(JsonConvert.SerializeObject(jsonResult.Value))["Errors"]?.FirstOrDefault());
        }

        [TestMethod]
        public async Task GetByNameAndBrandName_ShouldReturnJsonResult_WithOkHttpStatusCode()
        {
            // Arrange
            ProductDto productDto = new ProductDto { Id = 1, Name = "Product One", BrandName = "Brand One", Price = 30.00m };

            this._productService
                .Setup(ps => ps.GetProductByNameAndBrandAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(productDto);

            this._mapper
                .Setup(m => m.Map<ProductResponse>(It.IsAny<ProductDto>()))
                .Returns(new ProductResponse { Id = productDto.Id, Name = productDto.Name, BrandName = productDto.BrandName, Price = productDto.Price });

            this._productsController = new ProductsController(this._productService.Object, this._logger.Object, this._mapper.Object);

            // Act
            var jsonResult = await this._productsController.GetByNameAndBrandName(productDto.Name, productDto.BrandName, It.IsAny<CancellationToken>()).ConfigureAwait(false) as JsonResult;

            // Assert
            Assert.IsNotNull(jsonResult);
            Assert.AreEqual((int)HttpStatusCode.OK, jsonResult.StatusCode);

            var product = JObject.Parse(JsonConvert.SerializeObject(jsonResult.Value))["Product"];
            Assert.IsNotNull(product);
            Assert.AreEqual(product["Id"]?.Value<int>(), productDto.Id);
            Assert.AreEqual(product["Name"]?.Value<string>(), productDto.Name);
            Assert.AreEqual(product["BrandName"]?.Value<string>(), productDto.BrandName);
            Assert.AreEqual(product["Price"]?.Value<decimal>(), productDto.Price);
        }

        [TestMethod]
        public async Task Create_ShouldReturnJsonResult_WithBadRequestStatusCode_WhenProductWithSameNameAndBrandNameExists()
        {
            // Arrange
            ProductDto productDto = new ProductDto { Id = 1, Name = "Product One", BrandName = "Brand One", Price = 30.00m };

            this._productService
                .Setup(ps => ps.GetProductByNameAndBrandAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(productDto);

            this._mapper
                .Setup(m => m.Map<ProductResponse>(It.IsAny<ProductDto>()))
                .Returns(new ProductResponse { Id = productDto.Id, Name = productDto.Name, BrandName = productDto.BrandName, Price = productDto.Price });

            this._productsController = new ProductsController(this._productService.Object, this._logger.Object, this._mapper.Object);            

            // Act
            var jsonResult =
                await this._productsController.Create(
                    new ProductRequest
                    {
                        Name = productDto.Name,
                        BrandName = productDto.BrandName,
                        Price = productDto.Price
                    }, It.IsAny<CancellationToken>()).ConfigureAwait(false) as JsonResult;

            // Assert
            Assert.IsNotNull(jsonResult);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, jsonResult.StatusCode);
            Assert.AreEqual(
                $"Product with the same name '({productDto.Name})' and brand name '({productDto.BrandName})' already exists.",
                JObject.Parse(JsonConvert.SerializeObject(jsonResult.Value))["ValidationErrorMessages"]?.FirstOrDefault());
        }

        [TestMethod]
        public async Task Create_ShouldReturnJsonResult_WithOkStatusCode_WhenProductIsSuccessfullyCreated()
        {
            // Arrange
            ProductDto productDto = new ProductDto { Id = 1, Name = "Product One", BrandName = "Brand One", Price = 30.00m };

            this._productService
                .Setup(ps => ps.GetProductByNameAndBrandAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(It.IsAny<ProductDto>());

            this._productService
                .Setup(ps => ps.CreateProductAsync(It.IsAny<ProductDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(productDto);

            this._mapper
                .Setup(m => m.Map<ProductResponse>(productDto))
                .Returns(new ProductResponse { Id = productDto.Id, Name = productDto.Name, BrandName = productDto.BrandName, Price = productDto.Price });

            this._productsController = new ProductsController(this._productService.Object, this._logger.Object, this._mapper.Object);

            // Act
            var jsonResult =
                await this._productsController.Create(
                    new ProductRequest
                    {
                        Name = productDto.Name,
                        BrandName = productDto.BrandName,
                        Price = productDto.Price
                    }, It.IsAny<CancellationToken>()).ConfigureAwait(false) as JsonResult;

            // Assert
            Assert.IsNotNull(jsonResult);
            Assert.AreEqual((int)HttpStatusCode.Created, jsonResult.StatusCode);

            var product = JObject.Parse(JsonConvert.SerializeObject(jsonResult.Value))["Product"];
            Assert.IsNotNull(product);
            Assert.AreEqual(product["Id"]?.Value<int>(), productDto.Id);
            Assert.AreEqual(product["Name"]?.Value<string>(), productDto.Name);
            Assert.AreEqual(product["BrandName"]?.Value<string>(), productDto.BrandName);
            Assert.AreEqual(product["Price"]?.Value<decimal>(), productDto.Price);
        }

        [TestMethod]
        public async Task Update_ShouldReturnJsonResult_WithBadRequestStatusCode_WhenProductCannotBeFound()
        {
            // Arrange
            ProductDto productDto = new ProductDto { Id = 1, Name = "Product One", BrandName = "Brand One", Price = 30.00m };

            this._productService
                .Setup(ps => ps.GetProductByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(It.IsAny<ProductDto>());

            this._productsController = new ProductsController(this._productService.Object, this._logger.Object, this._mapper.Object);

            // Act
            var jsonResult =
                await this._productsController.Update(
                    It.IsAny<int>(),
                    new ProductRequest
                    {
                        Name = productDto.Name,
                        BrandName = productDto.BrandName,
                        Price = productDto.Price
                    }, It.IsAny<CancellationToken>()).ConfigureAwait(false) as JsonResult;


            // Assert
            Assert.IsNotNull(jsonResult);
            Assert.AreEqual((int)HttpStatusCode.NotFound, jsonResult.StatusCode);
            Assert.AreEqual($"We can't update requested product because requested product cannot be found",
                JObject.Parse(JsonConvert.SerializeObject(jsonResult.Value))["Errors"]?.FirstOrDefault());
        }

        [TestMethod]
        public async Task Update_ShouldReturnJsonResult_WithOkStatusCode_WhenProductIsSuccessfullyUpdated()
        {
            // Arrange
            ProductDto productDto = new ProductDto { Id = 1, Name = "Product One", BrandName = "Brand One", Price = 30.00m };
            this._productService
                .Setup(ps => ps.GetProductByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(productDto);
            this._mapper
                .Setup(m => m.Map<ProductResponse>(productDto))
                .Returns(new ProductResponse { Id = productDto.Id, Name = productDto.Name, BrandName = productDto.BrandName, Price = productDto.Price });

            
            ProductDto productToUpdate = new ProductDto { Id = 1, Name = "Product One", BrandName = "Brand Two", Price = 30.50m };
            this._productService
                .Setup(ps => ps.UpdateProductAsync(It.IsAny<ProductDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(productToUpdate);
            this._mapper
                .Setup(m => m.Map<ProductResponse>(productToUpdate))
                .Returns(new ProductResponse { Id = productToUpdate.Id, Name = productToUpdate.Name, BrandName = productToUpdate.BrandName, Price = productToUpdate.Price });

            this._productsController = new ProductsController(this._productService.Object, this._logger.Object, this._mapper.Object);

            // Act
            var jsonResult =
                await this._productsController.Update(
                    It.IsAny<int>(),
                    new ProductRequest
                    {
                        Name = productToUpdate.Name,
                        BrandName = productToUpdate.BrandName,
                        Price = productToUpdate.Price
                    }, It.IsAny<CancellationToken>()).ConfigureAwait(false) as JsonResult;

            // Assert
            Assert.IsNotNull(jsonResult);
            Assert.AreEqual((int)HttpStatusCode.OK, jsonResult.StatusCode);

            var product = JObject.Parse(JsonConvert.SerializeObject(jsonResult.Value))["Product"];
            Assert.IsNotNull(product);
            Assert.AreEqual(product["Id"]?.Value<int>(), productToUpdate.Id);
            Assert.AreEqual(product["Name"]?.Value<string>(), productToUpdate.Name);
            Assert.AreEqual(product["BrandName"]?.Value<string>(), productToUpdate.BrandName);
            Assert.AreEqual(product["Price"]?.Value<decimal>(), productToUpdate.Price);
        }

        [TestMethod]
        public async Task Delete_ShouldReturnJsonResult_WithBadRequestStatusCode_WhenProductCannotBeFound()
        {
            // Arrange
            ProductDto productDto = new ProductDto { Id = 1, Name = "Product One", BrandName = "Brand One", Price = 30.00m };

            this._productService
                .Setup(ps => ps.GetProductByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(It.IsAny<ProductDto>());

            this._productsController = new ProductsController(this._productService.Object, this._logger.Object, this._mapper.Object);

            // Act
            var jsonResult = await this._productsController.Delete(It.IsAny<int>(), It.IsAny<CancellationToken>()).ConfigureAwait(false) as JsonResult;


            // Assert
            Assert.IsNotNull(jsonResult);
            Assert.AreEqual((int)HttpStatusCode.NotFound, jsonResult.StatusCode);
            Assert.AreEqual($"We can't delete requested product because requested product cannot be found",
                JObject.Parse(JsonConvert.SerializeObject(jsonResult.Value))["Errors"]?.FirstOrDefault());
        }

        [TestMethod]
        public async Task Delete_ShouldReturnJsonResult_WithOkStatusCode_WhenProductIsSuccessfullyUpdated()
        {
            // Arrange
            ProductDto productDto = new ProductDto { Id = 1, Name = "Product One", BrandName = "Brand One", Price = 30.00m };
            this._productService
                .Setup(ps => ps.GetProductByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(productDto);
            this._mapper
                .Setup(m => m.Map<ProductResponse>(productDto))
                .Returns(new ProductResponse { Id = productDto.Id, Name = productDto.Name, BrandName = productDto.BrandName, Price = productDto.Price });

            this._productService
                .Setup(ps => ps.DeleteProductAsync(It.IsAny<ProductDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            this._productsController = new ProductsController(this._productService.Object, this._logger.Object, this._mapper.Object);

            // Act
            var jsonResult = await this._productsController.Delete(It.IsAny<int>(), It.IsAny<CancellationToken>()).ConfigureAwait(false) as JsonResult;

            // Assert
            Assert.IsNotNull(jsonResult);
            Assert.AreEqual((int)HttpStatusCode.OK, jsonResult.StatusCode);
            Assert.AreEqual("Requested product is deleted", JObject.Parse(JsonConvert.SerializeObject(jsonResult.Value))["Result"]);
        }
    }
}