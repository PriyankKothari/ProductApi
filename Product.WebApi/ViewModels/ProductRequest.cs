namespace ProductApi.WebApi.ViewModels
{
    /// <summary>
    /// Represents product request view model.
    /// </summary>
    public sealed class ProductRequest
    {
        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets Brand Name.
        /// </summary>
        public string BrandName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets Price.
        /// </summary>
        public decimal Price { get; set; }
    }
}