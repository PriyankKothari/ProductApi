using AutoMapper;
using Microsoft.Extensions.Logging;
using ProductApi.Application.DTOs;
using ProductApi.Domain.Entities;
using ProductApi.Domain.Repositories;

namespace ProductApi.Application.Services
{
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public sealed class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<IProductService> _logger;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initiates a new instance of <see cref="IProductService" />.
        /// </summary>
        /// <param name="productRepository"><see cref="IProductRepository" />.</param>
        /// <param name="logger"><see cref="ILogger{IProductService}" />.</param>
        /// <param name="mapper"><see cref="IMapper" />.</param>
        public ProductService(IProductRepository productRepository, ILogger<IProductService> logger, IMapper mapper)
        {
            this._productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this._mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public async Task<IEnumerable<ProductDto>> ListProductsAsync(CancellationToken cancellationToken)
        {
            try
            {
                return this._mapper.Map<IEnumerable<ProductDto>>(await this._productRepository.ListAsync(null, cancellationToken).ConfigureAwait(false));
            }
            catch (Exception exception)
            {
                this._logger.LogError(exception?.InnerException?.Message, exception);
                throw;
            }
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public async Task<IEnumerable<ProductDto>> ListProductsByBrandNameAsync(string brandName, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(brandName))
                throw new ArgumentNullException(nameof(brandName));

            try
            {
                return this._mapper.Map<IEnumerable<ProductDto>>(
                    await this._productRepository.ListAsync(pr => pr.BrandName.ToLower().Equals(brandName.ToLower()), cancellationToken).ConfigureAwait(false));
            }
            catch(Exception exception)
            {
                this._logger.LogError(exception?.InnerException?.Message, exception);
                throw;
            }
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public async Task<ProductDto?> GetProductByIdAsync(int id, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(id, nameof(id));

            try
            {
                return this._mapper.Map<ProductDto>(await this._productRepository.GetAsync(pr => pr.Id.Equals(id), cancellationToken).ConfigureAwait(false));
            }
            catch (Exception exception)
            {
                this._logger.LogError(exception?.InnerException?.Message, exception);
                throw;
            }
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public async Task<ProductDto?> GetProductByNameAsync(string name, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            try
            {
                return this._mapper.Map<ProductDto>(
                    await this._productRepository.GetAsync(pr => pr.Name.ToLower().Equals(name.ToLower()), cancellationToken).ConfigureAwait(false));
            }
            catch (Exception exception)
            {
                this._logger.LogError(exception?.InnerException?.Message, exception);
                throw;
            }
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public async Task<ProductDto?> GetProductByNameAndBrandAsync(string name, string brandName, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            if (string.IsNullOrWhiteSpace(brandName))
                throw new ArgumentNullException(nameof(brandName));

            try
            {
                return this._mapper.Map<ProductDto>(
                    await this._productRepository.GetAsync(
                        pr => pr.Name.ToLower().Contains(name.ToLower()) &&
                        pr.BrandName.ToLower().Contains(brandName.ToLower()), cancellationToken)
                    .ConfigureAwait(false));
            }
            catch (Exception exception)
            {
                this._logger.LogError(exception?.InnerException?.Message, exception);
                throw;
            }
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public async Task<ProductDto> CreateProductAsync(ProductDto productRequest, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(productRequest, nameof(productRequest));

            try
            {
                return this._mapper.Map<ProductDto>(await this._productRepository.CreateAsync(this._mapper.Map<Product>(productRequest), cancellationToken).ConfigureAwait(false));
            }
            catch (Exception exception)
            {
                this._logger.LogError(exception?.InnerException?.Message, exception);
                throw;
            }
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public async Task<ProductDto> UpdateProductAsync(ProductDto productRequest, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(productRequest, nameof(productRequest));

            try
            {
                return this._mapper.Map<ProductDto>(await this._productRepository.UpdateAsync(this._mapper.Map<Product>(productRequest), cancellationToken));
            }
            catch (Exception exception)
            {
                this._logger.LogError(exception?.InnerException?.Message, exception);
                throw;
            }
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public async Task<bool> DeleteProductAsync(ProductDto productRequest, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(productRequest, nameof(productRequest));

            try
            {
                return await this._productRepository.DeleteAsync(this._mapper.Map<Product>(productRequest), cancellationToken);
            }
            catch(Exception exception)
            {
                this._logger.LogError(exception?.InnerException?.Message, exception);
                throw;
            }
        }
    }
}