using Microsoft.AspNetCore.Mvc;
using ProductApi.Application.Services;
using System.Net;

namespace ProductApi.WebApi.Controllers
{
    /// <summary>
    /// Product controller.
    /// </summary>
    //[Route("v{version:apiVersion}/[controller]")]
    [Route("products")]
    [ApiController]
    public sealed class ProductsController : ControllerBase
    {
        //private readonly IProductService _productService;
        //private readonly ILogger<ProductsController> _logger;

        ///// <summary>
        ///// Initializes a new instance of <see cref="ProductsController" />.
        ///// </summary>
        ///// <param name="productService"><see cref="IProductService" />.</param>
        ///// <param name="logger"><see cref="ILogger{ProductController}" />.</param>
        ///// <exception cref="ArgumentNullException"></exception>
        //public ProductsController(IProductService productService, ILogger<ProductsController> logger)
        //{
        //    this._productService = productService ?? throw new ArgumentNullException(nameof(productService));
        //    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <response code = "200">Returns Ok</response>
        /// <response code = "400">Returns BadRequest</response>
        /// <response code = "500">Returns InternalServerError</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return new JsonResult(new { Data = "hello world", Errors = new List<string>() }) { StatusCode = (int)HttpStatusCode.OK };
            //throw new NotImplementedException();
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        [HttpGet("brand/{brandName}")]
        public async Task<IActionResult> GetByBrandName(string brandName)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{name}/brand/{brandName}")]
        public async Task<IActionResult> GetByNameAndBrandName(string name, string brandName)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<IActionResult> Create()
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        public async Task<IActionResult> Update()
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
