using Moq;
using Onyx.Product.Infrastructure.Repositories.Interfaces;
using Onyx.Products.Domain.Entities;
using Onyx.ProductsService;

namespace Onyx.Products.Tests.ProductServiceTests;

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly ProductService _productService;
    private Products.Domain.Entities.Product product;

    public ProductServiceTests()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _productService = new ProductService(_productRepositoryMock.Object);

        product = new Products.Domain.Entities.Product
        {
            Id = 1,
            Name = "Product1",
            Model = new ProductModel { Id = 1, ModelName = "Test" },
            Colour = new ProductColour { ColourName = "Red", Id = 1 },
            Price = new ProductPrice { Id = 1, Amount = 300 }
        };
    }

    [Fact]
    public async Task AddProductAsync_ShouldThrowArgumentNullException_WhenProductIsNull()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _productService.AddProductAsync(null));
    }

    [Fact]
    public async Task AddProductAsync_ShouldReturnProductId_WhenProductIsValid()
    {
        // Arrange
       
        _productRepositoryMock.Setup(repo => repo.AddAsync(product)).ReturnsAsync(product.Id);

        // Act
        var result = await _productService.AddProductAsync(product);

        // Assert
        Assert.Equal(product.Id, result);
    }

    [Fact]
    public async Task DeleteProductAsync_ShouldThrowArgumentNullException_WhenIdIsZero()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _productService.DeleteProductAsync(0));
    }

    [Fact]
    public async Task DeleteProductAsync_ShouldCallRepository_WhenIdIsValid()
    {
        // Arrange
        var productId = 1;
        _productRepositoryMock.Setup(repo => repo.DeleteAsync(productId)).Returns(Task.CompletedTask);

        // Act
        await _productService.DeleteProductAsync(productId);

        // Assert
        _productRepositoryMock.Verify(repo => repo.DeleteAsync(productId), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnListOfProducts()
    {
        // Arrange
        var products = new List<Products.Domain.Entities.Product> { product };
        _productRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(products);

        // Act
        var result = await _productService.GetAllAsync();

        // Assert
        Assert.Equal(products, result);
    }

    [Fact]
    public async Task GetByColourAsync_ShouldThrowArgumentNullException_WhenColourIsNullOrEmpty()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _productService.GetByColourAsync(null));
        await Assert.ThrowsAsync<ArgumentNullException>(() => _productService.GetByColourAsync(string.Empty));
    }

    [Fact]
    public async Task GetByColourAsync_ShouldReturnProducts_WhenColourIsValid()
    {
        // Arrange
        var colour = "Red";
        var products = new List<Products.Domain.Entities.Product> { product };
        _productRepositoryMock.Setup(repo => repo.GetByColourAsync(colour)).ReturnsAsync(products);

        // Act
        var result = await _productService.GetByColourAsync(colour);

        // Assert
        Assert.Equal(products, result);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldThrowArgumentNullException_WhenIdIsZero()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _productService.GetByIdAsync(0));
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnProduct_WhenIdIsValid()
    {
        // Arrange
        var productId = 1;
       
        _productRepositoryMock.Setup(repo => repo.GetByIdAsync(productId)).ReturnsAsync(product);

        // Act
        var result = await _productService.GetByIdAsync(productId);

        // Assert
        Assert.Equal(product, result);
    }

    [Fact]
    public async Task UpdateProductAsync_ShouldThrowArgumentNullException_WhenProductIsNull()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _productService.UpdateProductAsync(null));
    }

    [Fact]
    public async Task UpdateProductAsync_ShouldReturnUpdatedProduct_WhenProductIsValid()
    {
        // Arrange
       
        _productRepositoryMock.Setup(repo => repo.UpdateAsync(product)).ReturnsAsync(product);

        // Act
        var result = await _productService.UpdateProductAsync(product);

        // Assert
        Assert.Equal(product, result);
    }
}