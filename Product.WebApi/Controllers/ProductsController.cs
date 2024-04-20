using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
        /// <response code = "400">Returns BadRequest</response>
        /// <response code = "500">Returns InternalServerError</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            try
            {
                var products = this._mapper.Map<IEnumerable<ProductResponse>>(await this._productService.ListProductsAsync(cancellationToken).ConfigureAwait(false));
                return new JsonResult(new { Data = products, Errors = new List<string>(), StatusCode = (int)HttpStatusCode.OK });
            }
            catch (Exception exception)
            {
                this._logger.LogError(exception.Message, exception);
                return new JsonResult(new
                {
                    Data = default(ProductResponse),
                    Errors = $"{exception.Message}{exception.StackTrace}",
                    StatusCode = (int)HttpStatusCode.InternalServerError
                });
            }
        }

        /// <summary>
        /// Gets a product by id.
        /// </summary>
        /// <param name="id">Id of the product.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken "/>.</param>
        /// <returns><see cref="IActionResult" />.</returns>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(id, nameof(id));

            try
            {
                var product = this._mapper.Map<ProductResponse>(await this._productService.GetProductByIdAsync(id, cancellationToken).ConfigureAwait(false));

                if (product is null)
                    return new JsonResult(new
                    {
                        Data = default(ProductResponse),
                        Errors = $"We can't find the product by Id {id}",
                        StatusCode = (int)HttpStatusCode.NotFound
                    });

                return new JsonResult(new
                {
                    Data = product,
                    Errors = new List<string>(),
                    StatusCode = (int)HttpStatusCode.OK
                });
            }
            catch(Exception exception)
            {
                this._logger.LogError(exception.Message, exception);
                return new JsonResult(new
                {
                    Data = default(ProductResponse),
                    Errors = $"{exception.Message}{exception.StackTrace}",
                    StatusCode = (int)HttpStatusCode.InternalServerError
                });
            }
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetByName(string name, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        [HttpGet("brand/{brandName}")]
        public async Task<IActionResult> GetByBrandName(string brandName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{name}/brand/{brandName}")]
        public async Task<IActionResult> GetByNameAndBrandName(string name, string brandName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductRequest productRequestModel, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        public async Task<IActionResult> Update(ProductRequest productRequestModel, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
