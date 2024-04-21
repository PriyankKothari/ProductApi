using Microsoft.EntityFrameworkCore;
using ProductApi.Domain.Entities;

namespace ProductApi.Persistence.DatabaseContexts
{
    /// <summary>
    /// Database context class to declare <see cref="DbSet{TEntity}" />s, overriding <see cref="OnModelCreating(ModelBuilder)" /> configuration and <see cref="SaveChangesAsync(bool, CancellationToken)" /> method.
    /// </summary>
    public sealed class DatabaseContext : DbContext
    {
        /// <summary>
        /// Gets or sets Products.
        /// </summary>
        public DbSet <Product> Products { get; set; }

        /// <summary>
        /// Initiates a new instance of the <see cref="DatabaseContext" />.
        /// </summary>
        /// <param name="dbContextOptions"></param>
        public DatabaseContext(DbContextOptions<DatabaseContext> dbContextOptions) : base(dbContextOptions)
        {
            base.Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Product
            modelBuilder.Entity<Product>().ToTable("Product").HasKey(product => product.Id);
            modelBuilder.Entity<Product>().Property(product => product.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Product>().Property(product => product.Name).IsRequired();
            modelBuilder.Entity<Product>().Property(product => product.BrandName).IsRequired();
            modelBuilder.Entity<Product>().Property(product => product.Price).IsRequired();

            // setting unique key index for Product.Name and Product.BrandId to avoid duplicate Products.
            modelBuilder.Entity<Product>().HasIndex(product => new { product.Name, product.BrandName }).IsUnique(true);

            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken)
        {
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}
