namespace Onyx.ProductsService
{
    public interface IProductService
    {
        Task<Products.Domain.Entities.Product> GetByIdAsync(int id);
        Task<List<Products.Domain.Entities.Product>> GetAllAsync();
        Task<List<Products.Domain.Entities.Product>> GetByColourAsync(string colour);
        Task<int> AddProductAsync(Products.Domain.Entities.Product product);
        Task<Products.Domain.Entities.Product> UpdateProductAsync(Products.Domain.Entities.Product product);
        Task DeleteProductAsync(int id);
    }
}
