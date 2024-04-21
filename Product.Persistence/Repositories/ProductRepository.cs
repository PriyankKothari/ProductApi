using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductApi.Domain.Entities;
using ProductApi.Domain.Repositories;
using ProductApi.Persistence.DatabaseContexts;
using System.Linq.Expressions;

namespace ProductApi.Persistence.Repositories
{
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public sealed class ProductRepository : IProductRepository
    {
        private readonly DatabaseContext _databaseContext;
        private readonly ILogger<IProductRepository> _logger;

        public ProductRepository(DatabaseContext databaseContext, ILogger<IProductRepository> logger)
        {
            this._databaseContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public async Task<IEnumerable<Product>> ListAsync(Expression<Func<Product, bool>>? predicateExpression, CancellationToken cancellationToken)
        {
            try
            {
                if (predicateExpression is null)
                    return await this._databaseContext.Set<Product>().AsNoTracking().ToListAsync(cancellationToken).ConfigureAwait(false);

                return await this._databaseContext.Set<Product>().AsNoTracking().Where(predicateExpression).ToListAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                this._logger.LogCritical(exception?.InnerException?.Message, exception);
                throw;
            }
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public async Task<Product?> GetAsync(Expression<Func<Product, bool>> predicateExpression, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(predicateExpression, nameof(predicateExpression));

            try
            {
                return await this._databaseContext.Set<Product>().AsNoTracking().FirstOrDefaultAsync(predicateExpression).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                this._logger.LogCritical(exception?.InnerException?.Message, exception);
                throw;
            }
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public async Task<Product> CreateAsync(Product product, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(product, nameof(product));

            try
            {
                this._databaseContext.Set<Product>().Add(product);
                await this._databaseContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                return product;
            }
            catch (DbUpdateConcurrencyException concurrencyException)
            {
                this._logger.LogCritical(concurrencyException?.InnerException?.Message, concurrencyException);
                throw;
            }
            catch(Exception exception)
            {
                this._logger.LogCritical(exception?.InnerException?.Message, exception);
                throw;
            }
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public async Task<Product> UpdateAsync(Product product, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(product, nameof(product));

            try
            {
                var localProduct = this._databaseContext.Set<Product>().Local.FirstOrDefault(pr => pr.Id == product.Id);

                if (localProduct is not null)
                    this._databaseContext.Entry((object)localProduct).State = EntityState.Detached;

                this._databaseContext.Set<Product>().Update(product);
                await this._databaseContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                return product;
            }
            catch (DbUpdateConcurrencyException concurrencyException)
            {
                this._logger.LogCritical(concurrencyException?.InnerException?.Message, concurrencyException);
                throw;
            }
            catch (Exception exception)
            {
                this._logger.LogCritical(exception?.InnerException?.Message, exception);
                throw;
            }
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public async Task<bool> DeleteAsync(Product product, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(product, nameof(product));

            try
            {
                var localProduct = this._databaseContext.Set<Product>().Local.FirstOrDefault(pr => pr.Id == product.Id);

                if (localProduct is not null)
                    this._databaseContext.Entry((object)localProduct).State = EntityState.Detached;

                this._databaseContext.Set<Product>().Remove(product);
                await this._databaseContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                return true;
            }
            catch (DbUpdateConcurrencyException concurrencyException)
            {
                this._logger.LogCritical(concurrencyException?.InnerException?.Message, concurrencyException);
                throw;
            }
            catch (Exception exception)
            {
                this._logger.LogCritical(exception.Message, exception);
                throw;
            }
        }
    }
}
