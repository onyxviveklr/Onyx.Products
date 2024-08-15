using Microsoft.EntityFrameworkCore;
using Onyx.Products.Domain.Entities;

namespace ProductsApi.Infrastructure.Data
{
    public class ProductsDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductModel> ProductModels { get; set; }
        public DbSet<ProductPrice> ProductPrices { get; set; }
        public DbSet<ProductColour> ProductColours { get; set; }

        public ProductsDbContext(DbContextOptions<ProductsDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuring relationships and keys if needed
            modelBuilder.Entity<Product>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();
                
            modelBuilder.Entity<ProductModel>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<ProductPrice>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<ProductColour>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

            // Configuring foreign keys
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Model)
                .WithMany()
                .HasForeignKey("ModelId");

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Price)
                .WithMany()
                .HasForeignKey("PriceId");

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Colour)
                .WithMany()
                .HasForeignKey("ColourId");

            modelBuilder.Entity<ProductColour>().HasData(
                new ProductColour { Id = 1, ColourName = "Red" },
                new ProductColour { Id = 2, ColourName = "Blue" },
                new ProductColour { Id = 3, ColourName = "Yellow" });

            modelBuilder.Entity<ProductModel>().HasData(new ProductModel { Id = 1, ModelName = "iPhone 15 Pro" },
                new ProductModel { Id = 2, ModelName = "iPhone 15 Pro Max" },
                new ProductModel { Id = 3, ModelName = "Samsung Z Flip" },
                new ProductModel { Id = 4, ModelName = "Google Pixel 10" },
                new ProductModel { Id = 5, ModelName = "Google Pixel Pro" },
                new ProductModel { Id = 6, ModelName = "Google Pixel 10 Mini" });

            modelBuilder.Entity<ProductPrice>().HasData(new ProductPrice { Id = 1, Amount = 1000.0m },
                new ProductPrice { Id = 2, Amount = 1199.99m },
                new ProductPrice { Id = 3, Amount = 800.0m },
                new ProductPrice { Id = 4, Amount = 800.0m },
                new ProductPrice { Id = 5, Amount = 1800.0m },
                new ProductPrice { Id = 6, Amount = 800.0m });

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "iPhone", ModelId = 1, PriceId = 1, ColourId = 1 },
                new Product { Id = 2, Name = "iPhone", ModelId = 2, PriceId = 2, ColourId = 2 },
                new Product { Id = 3, Name = "Samsung Flip", ModelId = 3, PriceId = 3, ColourId = 3 },
                new Product { Id = 4, Name = "Google Pixel", ModelId = 4, PriceId = 4, ColourId = 1 },
                new Product { Id = 5, Name = "Google Pixel Pro", ModelId = 5, PriceId = 5, ColourId = 1 },
                new Product { Id = 6, Name = "Google Pixel Mini", ModelId = 6, PriceId = 6, ColourId = 3 }
                );
        }
    }
}
