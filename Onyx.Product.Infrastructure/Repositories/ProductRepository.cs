using Onyx.Product.Infrastructure.Repositories.Interfaces;
using ProductsApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore; // Required for Include extension method

namespace Onyx.Product.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductsDbContext _context;

        public ProductRepository(ProductsDbContext context)
        {
            _context = context;
        }

        public async Task<Products.Domain.Entities.Product> GetByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Model)
                .Include(p => p.Price)
                .Include(p => p.Colour)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Products.Domain.Entities.Product>> GetAllAsync()
        {
            return await _context.Products
                .Include(p => p.Model)
                .Include(p => p.Price)
                .Include(p => p.Colour)
                .ToListAsync();
        }

        public async Task<List<Products.Domain.Entities.Product>> GetByColourAsync(string colour)
        {
            return await _context.Products
                .Include(p => p.Model)
                .Include(p => p.Price)
                .Include(p => p.Colour)
                .Where(p => p.Colour.ColourName == colour)
                .ToListAsync();
        }

        public async Task<int> AddAsync(Products.Domain.Entities.Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product.Id;
        }

        public async Task<Products.Domain.Entities.Product> UpdateAsync(Products.Domain.Entities.Product product)
        {
            {
                var existingProduct = _context.Products
                    .Include(p => p.Model)
                    .Include(p => p.Price)
                    .Include(p => p.Colour).Where(p => p.Id == product.Id).FirstOrDefault();

                if (existingProduct == null)
                {
                    throw new ArgumentException("Product not found");
                }

                // Update the properties
                _context.Entry(existingProduct).CurrentValues.SetValues(product);

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    // Handle concurrency exception
                    Console.WriteLine("Concurrency exception: " + ex.Message);
                    throw;
                }
                catch (Exception ex)
                {
                    // Handle other exceptions
                    Console.WriteLine("An error occurred: " + ex.Message);
                    throw;
                }

                return existingProduct;
            }
        }

        public async Task DeleteAsync(int id)
        {
            var product = await GetByIdAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}
