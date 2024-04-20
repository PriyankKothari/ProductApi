namespace ProductApi.Domain.Entities
{
    /// <summary>
    /// Represents brand entity.
    /// </summary>
    public sealed class Brand
    {
        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }
}