using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Application.DTOs;
using ProductApi.Application.Services;
using ProductApi.WebApi.ViewModels;
using System.Net;

namespace ProductApi.WebApi.Controllers
{
    /// <summary>
    /// Product controller.
    /// </summary>
    [Route("v{version:apiVersion}/products")]
    [ApiVersion("1.0")]
    [ApiController]
    public sealed class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductsController> _logger;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of <see cref="ProductsController" />.
        /// </summary>
        /// <param name="productService"><see cref="IProductService" />.</param>
        /// <param name="logger"><see cref="ILogger{ProductController}" />.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ProductsController(IProductService productService, ILogger<ProductsController> logger, IMapper mapper)
        {
            this._productService = productService ?? throw new ArgumentNullException(nameof(productService));
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this._mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Lists all products.
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken" />.</param>
        /// <returns><see cref="IActionResult" />.</returns>
        /// <response code = "200">Returns Ok</response>
        /// <response code = "500">Returns InternalServerError</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            try
            {
                var products = this._mapper.Map<IEnumerable<ProductResponse>>(await this._productService.ListProductsAsync(cancellationToken).ConfigureAwait(false));
                return new JsonResult(new { Products = products }) { StatusCode = (int)HttpStatusCode.OK };
            }
            catch (Exception exception)
            {
                this._logger.LogError(exception?.InnerException?.Message, exception);

                return new JsonResult(new
                // return data and errors
                {
                    Products = default(IEnumerable<ProductResponse>),
                    Errors = new List<string> { $"{exception?.InnerException?.Message}" }
                })
                // return status code
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
        }

        /// <summary>
        /// Gets a product by id.
        /// </summary>
        /// <param name="id">Id of the product.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken "/>.</param>
        /// <returns><see cref="IActionResult" />.</returns>
        /// <response code = "200">Returns Ok</response>
        /// <response code = "404">Returns NotFound</response>
        /// <response code = "500">Returns InternalServerError</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(id, nameof(id));

            try
            {
                var product = this._mapper.Map<ProductResponse>(await this._productService.GetProductByIdAsync(id, cancellationToken).ConfigureAwait(false));

                if (product is null)
                {
                    return new JsonResult(new
                    // return data and errors
                    {
                        Product = default(ProductResponse),
                        Errors = new List<string> { $"We can't find any product by Id '{id}'" }
                    })
                    // return status code
                    { StatusCode = (int)HttpStatusCode.NotFound };
                }

                return new JsonResult(new { Product = product }) { StatusCode = (int)HttpStatusCode.OK };
            }
            catch(Exception exception)
            {
                this._logger.LogError(exception.Message, exception);
                
                return new JsonResult(new
                // return data and errors
                {
                    Product = default(ProductResponse),
                    Errors = new List<string> { $"{exception?.InnerException?.Message}" }
                })
                // return status code
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
        }

        /// <summary>
        /// Gets a product by name.
        /// </summary>
        /// <param name="name">Name of the product.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken" />.</param>
        /// <returns><see cref="IActionResult" />.</returns>
        /// <response code = "200">Returns Ok</response>
        /// <response code = "404">Returns NotFound</response>
        /// <response code = "500">Returns InternalServerError</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [HttpGet("{name}")]
        public async Task<IActionResult> GetByName(string name, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(name, nameof(name));

            try
            {
                var product = this._mapper.Map<ProductResponse>(await this._productService.GetProductByNameAsync(name, cancellationToken).ConfigureAwait(false));

                if (product is null)
                {
                    return new JsonResult(new
                    // return data and errors
                    {
                        Product = default(ProductResponse),
                        Errors = new List<string> { $"We can't find any product by Name '{name}'" }
                    })
                    // return status code
                    { StatusCode = (int)HttpStatusCode.NotFound };
                }

                return new JsonResult(new { Product = product }) { StatusCode = (int)HttpStatusCode.OK };
            }
            catch (Exception exception)
            {
                this._logger.LogError(exception.Message, exception);

                return new JsonResult(new
                // return data and errors
                {
                    Product = default(ProductResponse),
                    Errors = new List<string> { $"{exception?.InnerException?.Message}" }
                })
                // return status code
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
        }

        /// <summary>
        /// Gets products by brand name.
        /// </summary>
        /// <param name="brandName">Brand name of the product.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken" />.</param>
        /// <returns><see cref="IActionResult" />.</returns>
        /// <response code = "200">Returns Ok</response>
        /// <response code = "500">Returns InternalServerError</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [HttpGet("brands/{brandName}")]
        public async Task<IActionResult> GetByBrandName(string brandName, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(brandName, nameof(brandName));

            try
            {
                var products = this._mapper.Map<IEnumerable<ProductResponse>>(await this._productService.ListProductsByBrandNameAsync(brandName, cancellationToken).ConfigureAwait(false));
                return new JsonResult(new { Products = products }) { StatusCode = (int)HttpStatusCode.OK };
            }
            catch (Exception exception)
            {
                this._logger.LogError(exception.Message, exception);

                return new JsonResult(new
                // return data and errors
                {
                    Products = default(IEnumerable<ProductResponse>),
                    Errors = new List<string> { $"{exception?.InnerException?.Message}" }
                })
                // return status code
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
        }

        /// <summary>
        /// Gets a product by product name and brand name.
        /// </summary>
        /// <param name="name">Name of the product.</param>
        /// <param name="brandName">Brand name of the product.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken" />.</param>
        /// <returns><see cref="IActionResult" />.</returns>
        /// <response code = "200">Returns Ok</response>
        /// <response code = "404">Returns NotFound</response>
        /// <response code = "500">Returns InternalServerError</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [HttpGet("{name}/brands/{brandName}")]
        public async Task<IActionResult> GetByNameAndBrandName(string name, string brandName, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(name, nameof(name));
            ArgumentNullException.ThrowIfNull(brandName, nameof(brandName));

            try
            {
                var product = this._mapper.Map<ProductResponse>(await this._productService.GetProductByNameAndBrandAsync(name, brandName, cancellationToken).ConfigureAwait(false));

                if (product is null)
                {
                    return new JsonResult(new
                    // return data and errors
                    {
                        Product = default(ProductResponse),
                        Errors = new List<string> { $"We can't find the product by Name '{name}' and Brand Name '{brandName}'" }
                    })
                    // return status code
                    { StatusCode = (int)HttpStatusCode.NotFound };
                }

                return new JsonResult(new { Product = product }) { StatusCode = (int)HttpStatusCode.OK };
            }
            catch (Exception exception)
            {
                this._logger.LogError(exception.Message, exception);

                return new JsonResult(new
                // return data and errors
                {
                    Product = default(ProductResponse),
                    Errors = new List<string> { $"{exception?.InnerException?.Message}" }
                })
                // return status code
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
        }

        /// <summary>
        /// Creates a product.
        /// </summary>
        /// <param name="productRequestModel"><see cref="ProductRequest" />.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken" />.</param>
        /// <returns><see cref="ProductResponse" />.</returns>
        /// <response code = "201">Returns Created</response>
        /// <response code = "400">Returns BadRequest</response>
        /// <response code = "404">Returns NotFound</response>
        /// <response code = "500">Returns InternalServerError</response>
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]ProductRequest productRequestModel, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(productRequestModel, nameof(productRequestModel));

            try
            {
                var existingProduct =
                    this._mapper.Map<ProductResponse>(await this._productService.GetProductByNameAndBrandAsync(productRequestModel.Name, productRequestModel.BrandName, cancellationToken));

                if (existingProduct is not null)
                {
                    return new JsonResult(new
                    // return data and errors
                    {
                        ValidationErrorMessages = new List<string> { $"Product with the same name '({productRequestModel.Name})' and brand name '({productRequestModel.BrandName})' already exists." }
                    })
                    // return status code
                    { StatusCode = (int)HttpStatusCode.BadRequest };
                }

                var product = this._mapper.Map<ProductResponse>(await this._productService.CreateProductAsync(this._mapper.Map<ProductDto>(productRequestModel), cancellationToken));

                if (product is null)
                {
                    return new JsonResult(new
                    // return data and errors
                    {
                        Errors = new List<string> { $"We can't create a new product (Name: {productRequestModel.Name}, BrandName: {productRequestModel.BrandName}, Price: {productRequestModel.Price}" }
                    })
                    // return status code
                    { StatusCode = (int)HttpStatusCode.InternalServerError };
                }

                return new JsonResult(new { Product = product }) { StatusCode = (int)HttpStatusCode.Created };
            }
            catch (Exception exception)
            {
                this._logger.LogError(exception.Message, exception);

                return new JsonResult(new
                // return data and errors
                {
                    Product = default(ProductResponse),
                    Errors = new List<string> { $"{exception?.InnerException?.Message}" }
                })
                // return status code
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
        }

        /// <summary>
        /// Updates a product.
        /// </summary>
        /// <param name="id">Id of the product.</param>
        /// <param name="productRequestModel"><see cref="ProductRequest" />.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken" />.</param>
        /// <returns><see cref="ProductResponse" />.</returns>
        /// <response code = "200">Returns Ok</response>
        /// <response code = "400">Returns BadRequest</response>
        /// <response code = "404">Returns NotFound</response>
        /// <response code = "500">Returns InternalServerError</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, ProductRequest productRequestModel, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(productRequestModel, nameof(productRequestModel));

            try
            {
                var productToUpdate = this._mapper.Map<ProductResponse>(await this._productService.GetProductByIdAsync(id, cancellationToken));

                if (productToUpdate is null)
                {
                    return new JsonResult(new
                    // return data and errors
                    {
                        Errors = new List<string> { $"We can't update requested product because requested product cannot be found" }
                    })
                    // return status code
                    { StatusCode = (int)HttpStatusCode.NotFound };
                }

                var product =
                    this._mapper.Map<ProductResponse>(
                        await this._productService.UpdateProductAsync(
                            new ProductDto
                            {
                                Id = id,
                                Name = productRequestModel.Name,
                                BrandName = productRequestModel.BrandName,
                                Price = productRequestModel.Price
                            }, cancellationToken));

                if (product is null)
                {
                    return new JsonResult(new
                    // return data and errors
                    {
                        Product = default(ProductResponse),
                        Errors = new List<string> { $"We can't updated requested product because requested product cannot be found" }
                    })
                    // return status code
                    { StatusCode = (int)HttpStatusCode.NotFound };
                }

                return new JsonResult(new { Product = product }) { StatusCode = (int)HttpStatusCode.OK };
            }
            catch (Exception exception)
            {
                this._logger.LogError(exception.Message, exception);

                return new JsonResult(new
                // return data and errors
                {
                    Product = default(ProductResponse),
                    Errors = new List<string> { $"{exception?.InnerException?.Message}" }
                })
                // return status code
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
        }

        /// <summary>
        /// Deletes a product.
        /// </summary>
        /// <param name="id">Id of the product.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken" />.</param>
        /// <returns><see langword="true" /> or <see langword="false" />.</returns>
        /// <response code = "200">Returns Ok</response>
        /// <response code = "404">Returns NotFound</response>
        /// <response code = "500">Returns InternalServerError</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(id, nameof(id));

            try
            {
                var productToDelete = this._mapper.Map<ProductResponse>(await this._productService.GetProductByIdAsync(id, cancellationToken));

                if (productToDelete is null)
                {
                    return new JsonResult(new
                    // return data and errors
                    {
                        Errors = new List<string> { $"We can't delete requested product because requested product cannot be found" }
                    })
                    // return status code
                    { StatusCode = (int)HttpStatusCode.NotFound };
                }

                bool isProductDeleted =
                    await this._productService.DeleteProductAsync(
                        new ProductDto
                        {
                            Id  = id,
                            Name = productToDelete.Name,
                            BrandName = productToDelete.BrandName,
                            Price = productToDelete.Price
                        }, cancellationToken);

                return new JsonResult(new
                // return deleted message
                {
                    Result = isProductDeleted ? "Requested product is deleted" : "Requested product cannot be deleted"
                })
                // return status code
                { StatusCode = (int)HttpStatusCode.OK };
            }
            catch (Exception exception)
            {
                this._logger.LogError(exception.Message, exception);

                return new JsonResult(new
                // return data and errors
                {
                    Product = default(ProductResponse),
                    Errors = new List<string> { $"{exception?.InnerException?.Message}" }
                })
                // return status code
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
        }
    }
}