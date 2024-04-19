using Microsoft.Extensions.Logging;
using ProductApi.Domain.Entities;
using ProductApi.Domain.Repositories;
using System.Linq.Expressions;

namespace ProductApi.Application.Services
{
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public sealed class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<IProductService> _logger;

        /// <summary>
        /// Initiates a new instance of <see cref="IProductService" />.
        /// </summary>
        /// <param name="productRepository"></param>
        public ProductService(IProductRepository productRepository, ILogger<IProductService> logger)
        {
            this._productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public async Task<bool> ProductExistsAsync(Expression<Func<Product, bool>> predicateExpression, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(predicateExpression, nameof(predicateExpression));

            try
            {
                return await this._productRepository.ExistsAsync(predicateExpression, cancellationToken);
            }
            catch(Exception exception)
            {
                this._logger.LogError(exception.Message, exception);
                throw;
            }
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public async Task<List<Product>> ListProductsAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await this._productRepository.ListAsync(null, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                this._logger.LogError(exception.Message, exception);
                throw;
            }
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public async Task<List<Product>> ListProductsByBrandNameAsync(string brandName, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(brandName))
                throw new ArgumentNullException(nameof(brandName));

            try
            {
                return await this._productRepository.ListAsync(pr => pr.Brand.Name.Equals(brandName, StringComparison.OrdinalIgnoreCase), cancellationToken).ConfigureAwait(false);
            }
            catch(Exception exception)
            {
                this._logger.LogError(exception.Message, exception);
                throw;
            }
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public async Task<Product?> GetProductByIdAsync(int id, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(id, nameof(id));

            try
            {
                return await this._productRepository.GetAsync(pr => pr.Id.Equals(id), cancellationToken).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                this._logger.LogError(exception.Message, exception);
                throw;
            }
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public async Task<Product?> GetProductByNameAsync(string name, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            try
            {
                return await this._productRepository.GetAsync(pr => pr.Name.Equals(name, StringComparison.OrdinalIgnoreCase), cancellationToken).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                this._logger.LogError(exception.Message, exception);
                throw;
            }
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public async Task<Product?> GetProductByNameAndBrandAsync(string name, string brandName, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            if (string.IsNullOrWhiteSpace(brandName))
                throw new ArgumentNullException(nameof(brandName));

            try
            {
                return await this._productRepository.GetAsync(pr => pr.Name.Contains(name) && pr.Brand.Name.Contains(brandName), cancellationToken).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                this._logger.LogError(exception.Message, exception);
                throw;
            }
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public async Task<Product> CreateProductAsync(Product product, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(product, nameof(product));

            try
            {
                return await this._productRepository.CreateAsync(product, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                this._logger?.LogError(exception.Message, exception);
                throw;
            }
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public async Task<Product> UpdateProductAsync(Product product, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(product, nameof(product));

            try
            {
                return await this._productRepository.UpdateAsync(product, cancellationToken);
            }
            catch (Exception exception)
            {
                this._logger.LogError(exception.Message, exception);
                throw;
            }
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public async Task<bool> DeleteProductAsync(Product product, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(product, nameof(product));

            try
            {
                return await this._productRepository.DeleteAsync(product, cancellationToken);
            }
            catch(Exception exception)
            {
                this._logger.LogError(exception.Message, exception);
                throw;
            }
        }
    }
}