﻿namespace ProductApi.Domain.Entities
{
    /// <summary>
    /// Represents product entity.
    /// </summary>
    public sealed class Product
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
        /// Gets or sets Brand.
        /// </summary>
        public Brand Brand { get; set; } = new Brand();

        /// <summary>
        /// Gets or sets Price.
        /// </summary>
        public decimal Price { get; set; }
    }
}