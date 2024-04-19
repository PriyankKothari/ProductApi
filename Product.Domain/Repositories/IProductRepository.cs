using ProductApi.Domain.Entities;
using System.Linq.Expressions;

namespace ProductApi.Domain.Repositories
{
    /// <summary>
    /// Represents repository for <see cref="Product" />.
    /// </summary>
    public interface IProductRepository
    {
        /// <summary>
        /// Checks if a <see cref="Product" /> exists that matches the predicate expression.
        /// </summary>
        /// <param name="predicateExpression"><see cref="Expression{TDelegate}" />.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken" />.</param>
        /// <returns><see langword="true" /> or <see langword="false" />.</returns>
        Task<bool> ExistsAsync(Expression<Func<Product, bool>> predicateExpression, CancellationToken cancellationToken);

        /// <summary>
        /// Gets a <see cref="List{Product}" /> that matches the predicate expression.
        /// </summary>
        /// <param name="predicateExpression"><see cref="Expression{TDelegate}" />.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken" />.</param>
        /// <returns><see cref="List{Product}" />.</returns>
        Task<List<Product>> ListAsync(Expression<Func<Product, bool>>? predicateExpression, CancellationToken cancellationToken);

        /// <summary>
        /// Gets a <see cref="Product" /> that matches the predicate expression.
        /// </summary>
        /// <param name="predicateExpression"><see cref="Expression{TDelegate}" />.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken" />.</param>
        /// <returns><see cref="Product" />.</returns>
        Task<Product?> GetAsync(Expression<Func<Product, bool>>? predicateExpression, CancellationToken cancellationToken);

        /// <summary>
        /// Creates a <see cref="Product" />.
        /// </summary>
        /// <param name="product"><see cref="Product" />.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken" />.</param>
        /// <returns>A new <see cref="Product" />.</returns>
        Task<Product> CreateAsync(Product product, CancellationToken cancellationToken);

        /// <summary>
        /// Updates a <see cref="Product" />.
        /// </summary>
        /// <param name="product"><see cref="Product" />.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken" />.</param>
        /// <returns>An updated <see cref="Product" />.</returns>
        Task<Product> UpdateAsync(Product product, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes a <see cref="Product" />.
        /// </summary>
        /// <param name="product"><see cref="Product" />.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken" />.</param>
        /// <returns><see langword="true" /> or <see langword="false"/>.</returns>
        Task<bool> DeleteAsync(Product product, CancellationToken cancellationToken);
    }
}
