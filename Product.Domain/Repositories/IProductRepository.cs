using ProductApi.Domain.Entities;
using System.Linq.Expressions;

namespace ProductApi.Domain.Repositories
{
    /// <summary>
    /// Represents repository service for <see cref="Product" />.
    /// </summary>
    public interface IProductRepository
    {
        /// <summary>
        /// Checks if a <see cref="Product" /> exists that matches the predicate expression.
        /// </summary>
        /// <param name="predicateExpression"><see cref="Expression{TDelegate}" />.</param>
        /// <returns><see langword="true" /> or <see langword="false" />.</returns>
        Task<bool> ExistsAsync(Expression<Func<Product, bool>> predicateExpression);

        /// <summary>
        /// Gets a <see cref="List{Product}" /> that matches the predicate expression.
        /// </summary>
        /// <param name="predicateExpression"><see cref="Expression{TDelegate}" />.</param>
        /// <returns><see cref="List{Product}" />.</returns>
        Task<List<Product>> ListAsync(Predicate<Func<Product, bool>>? predicateExpression);

        /// <summary>
        /// Gets a <see cref="Product" /> that matches the predicate expression.
        /// </summary>
        /// <param name="predicateExpression"><see cref="Expression{TDelegate}" />.</param>
        /// <returns><see cref="Product" />.</returns>
        Task<Product> GetAsync(Predicate<Func<Product, bool>>? predicateExpression);

        /// <summary>
        /// Creates a <see cref="Product" />.
        /// </summary>
        /// <param name="product"><see cref="Product" />.</param>
        /// <returns><see cref="Product" />.</returns>
        Task<Product> CreateAsync(Product product);

        /// <summary>
        /// Updates a <see cref="Product" />.
        /// </summary>
        /// <param name="product"><see cref="Product" />.</param>
        /// <returns><see cref="Product" />.</returns>
        Task<Product> UpdateAsync(Product product);

        /// <summary>
        /// Deletes a <see cref="Product" />.
        /// </summary>
        /// <param name="product"><see cref="Product" />.</param>
        /// <returns><see langword="true" /> or <see langword="false"/>.</returns>
        Task<bool> DeleteAsync(Product product);
    }
}
