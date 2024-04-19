using ProductApi.Domain.Entities;
using System.Linq.Expressions;

namespace ProductApi.Application.Services
{
    /// <summary>
    /// Represents service for <see cref="Product" />.
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        /// Checks if a <see cref="Product" /> exists that matches the predicate expression.
        /// </summary>
        /// <param name="predicateExpression"><see cref="Expression{TDelegate}" />.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken" />.</param>
        /// <returns><see langword="true" /> or <see langword="false" />.</returns>
        Task<bool> ProductExistsAsync(Expression<Func<Product, bool>> predicateExpression, CancellationToken cancellationToken);

        /// <summary>
        /// Gets a <see cref="List{Product}" />.
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken" />.</param>
        /// <returns><see cref="List{Product}" />.</returns>
        Task<List<Product>> ListProductsAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Gets a <see cref="List{Product}" /> that matches the brand name.
        /// </summary>
        /// <param name="brandName">Brand name of a product.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken" />.</param>
        /// <returns><see cref="List{Product}" />.</returns>
        Task<List<Product>> ListProductsByBrandNameAsync(string brandName, CancellationToken cancellationToken);

        /// <summary>
        /// Gets a <see cref="Product" /> by product id.
        /// </summary>
        /// <param name="id">Id of the product.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken" />.</param>
        /// <returns><see cref="Product" />.</returns>
        Task<Product?> GetProductByIdAsync(int id, CancellationToken cancellationToken);

        /// <summary>
        /// Gets a <see cref="Product" /> by product name.
        /// </summary>
        /// <param name="name">Name of the product.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken" />.</param>
        /// <returns><see cref="Product" />.</returns>
        Task<Product?> GetProductByNameAsync(string name, CancellationToken cancellationToken);

        /// <summary>
        /// Gets a <see cref="Product" /> by product name and brand name.
        /// </summary>
        /// <param name="name">Name of the product.</param>
        /// <param name="brandName">Name of the product brand.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken" />.</param>
        /// <returns><see cref="Product" />.</returns>
        Task<Product?> GetProductByNameAndBrandAsync(string name, string brandName, CancellationToken cancellationToken);

        /// <summary>
        /// Creates a <see cref="Product" />.
        /// </summary>
        /// <param name="product"><see cref="Product" />.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken" />.</param>
        /// <returns>A new <see cref="Product" />.</returns>
        Task<Product> CreateProductAsync(Product product, CancellationToken cancellationToken);

        /// <summary>
        /// Updates a <see cref="Product" />.
        /// </summary>
        /// <param name="product"><see cref="Product" />.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken" />.</param>
        /// <returns>An updated <see cref="Product" />.</returns>
        Task<Product> UpdateProductAsync(Product product, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes a <see cref="Product" />.
        /// </summary>
        /// <param name="product"><see cref="Product" />.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken" />.</param>
        /// <returns><see langword="true" /> or <see langword="false" />.</returns>
        Task<bool> DeleteProductAsync(Product product, CancellationToken cancellationToken);
    }
}
