using Onyx.Product.Infrastructure.Repositories.Interfaces;

namespace Onyx.ProductsService
{
    public class ProductService : IProductService
    {
        public readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<int> AddProductAsync(Products.Domain.Entities.Product product)
        {
            if(product == null)
            {
                throw new ArgumentNullException();
            }

            return await _productRepository.AddAsync(product);
        }

        public async Task DeleteProductAsync(int id)
        {
            if(id == 0)
            {
                throw new ArgumentNullException();
            }

            await _productRepository.DeleteAsync(id);
        }

        public async Task<List<Products.Domain.Entities.Product>> GetAllAsync()
        {
            return await _productRepository.GetAllAsync();
        }

        public async Task<List<Products.Domain.Entities.Product>> GetByColourAsync(string colour)
        {
            if(string.IsNullOrEmpty(colour))
            {
                throw new ArgumentNullException();
            }
            return await _productRepository.GetByColourAsync(colour);
        }

        public async Task<Products.Domain.Entities.Product> GetByIdAsync(int id)
        {
            if (id == 0)
            {
                throw new ArgumentNullException();
            }

            return await _productRepository.GetByIdAsync(id);
        }

        public async Task<Products.Domain.Entities.Product> UpdateProductAsync(Products.Domain.Entities.Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException();
            }

            return await _productRepository.UpdateAsync(product);
        }
    }
}
