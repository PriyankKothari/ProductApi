namespace ProductApi.Application.DTOs
{
    public sealed class ProductDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string BrandName { get; set; } = string.Empty;

        public decimal Price { get; set; }
    }
}