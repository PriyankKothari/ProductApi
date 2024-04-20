namespace ProductApi.WebApi.ViewModels
{
    /// <summary>
    /// Represents product response view model.
    /// </summary>
    public sealed class ProductResponse
    {
        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets BrandName.
        /// </summary>
        public string BrandName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets Price.
        /// </summary>
        public decimal Price { get; set; }
    }
}