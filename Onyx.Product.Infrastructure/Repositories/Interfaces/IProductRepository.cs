namespace Onyx.Product.Infrastructure.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<Products.Domain.Entities.Product> GetByIdAsync(int id);
        Task<List<Products.Domain.Entities.Product>> GetAllAsync();
        Task<List<Products.Domain.Entities.Product>> GetByColourAsync(string colour);
        Task<int> AddAsync(Products.Domain.Entities.Product product);
        Task<Products.Domain.Entities.Product> UpdateAsync(Products.Domain.Entities.Product product);
        Task DeleteAsync(int id);
    }
}
