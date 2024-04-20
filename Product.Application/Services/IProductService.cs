using ProductApi.Application.DTOs;

namespace ProductApi.Application.Services
{
    /// <summary>
    /// Represents service for products.
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        /// Gets a list of products.
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken" />.</param>
        /// <returns><see cref="List{T}" />.</returns>
        Task<IEnumerable<ProductDto>> ListProductsAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Gets a product by brand name.
        /// </summary>
        /// <param name="brandName">Brand name of a product.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken" />.</param>
        /// <returns><see cref="List{T}" />.</returns>
        Task<IEnumerable<ProductDto>> ListProductsByBrandNameAsync(string brandName, CancellationToken cancellationToken);

        /// <summary>
        /// Gets a product by id.
        /// </summary>
        /// <param name="id">Id of the product.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken" />.</param>
        /// <returns><see cref="ProductDto" />.</returns>
        Task<ProductDto?> GetProductByIdAsync(int id, CancellationToken cancellationToken);

        /// <summary>
        /// Gets a product by name.
        /// </summary>
        /// <param name="name">Name of the product.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken" />.</param>
        /// <returns><see cref="ProductDto" />.</returns>
        Task<ProductDto?> GetProductByNameAsync(string name, CancellationToken cancellationToken);

        /// <summary>
        /// Gets a product by product name and brand name.
        /// </summary>
        /// <param name="name">Name of the product.</param>
        /// <param name="brandName">Name of the product brand.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken" />.</param>
        /// <returns><see cref="ProductDto" />.</returns>
        Task<ProductDto?> GetProductByNameAndBrandAsync(string name, string brandName, CancellationToken cancellationToken);

        /// <summary>
        /// Creates a product.
        /// </summary>
        /// <param name="productRequest"><see cref="ProductDto" />.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken" />.</param>
        /// <returns><see cref="ProductDto" />.</returns>
        Task<ProductDto> CreateProductAsync(ProductDto productRequest, CancellationToken cancellationToken);

        /// <summary>
        /// Updates a product.
        /// </summary>
        /// <param name="productRequest"><see cref="ProductDto" />.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken" />.</param>
        /// <returns><see cref="ProductDto" />.</returns>
        Task<ProductDto> UpdateProductAsync(ProductDto productRequest, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes a product.
        /// </summary>
        /// <param name="productRequest"><see cref="ProductDto" />.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken" />.</param>
        /// <returns><see langword="true" /> or <see langword="false" />.</returns>
        Task<bool> DeleteProductAsync(ProductDto productRequest, CancellationToken cancellationToken);
    }
}
