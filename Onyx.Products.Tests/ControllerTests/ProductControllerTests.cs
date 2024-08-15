using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Onyx.Products.Domain.Entities;
using Onyx.ProductsApi.Commands;
using Onyx.ProductsApi.Dto;
using Onyx.ProductsApi.Queries;
using Products.Controllers;

namespace Onyx.Products.Tests.ControllerTests;

public class ProductControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILogger<ProductController>> _loggerMock;
    private readonly ProductController _controller;
    private Products.Domain.Entities.Product product;
    public ProductControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILogger<ProductController>>();
        _controller = new ProductController(_mapperMock.Object, _mediatorMock.Object, _loggerMock.Object);

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
    public async Task GetProducts_ReturnsOkResult_WithListOfProducts()
    {
        // Arrange
        var products = new List<Products.Domain.Entities.Product> { product };
       
        var productsDto = new List<ProductDto> { new ProductDto { Id = 1, Name = "Product1" } };

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetProductsQuery>(), default)).ReturnsAsync(products);
        _mapperMock.Setup(m => m.Map<IEnumerable<ProductDto>>(products)).Returns(productsDto);

        // Act
        var result = await _controller.GetProducts();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<List<ProductDto>>(okResult.Value);
        Assert.Single(returnValue);
    }

    [Fact]
    public async Task CreateProduct_ReturnsOkResult_WithProductId()
    {
        // Arrange
        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateProductCommand>(), default)).ReturnsAsync(product.Id);

        // Act
        var result = await _controller.CreateProduct(product);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal($"Product - {product.Id} successfully created!", okResult.Value);
    }

    [Fact]
    public async Task DeleteProduct_ReturnsOkResult_WithProductId()
    {
        // Arrange
        var productId = 1;
        _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteProductCommand>(), default)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteProduct(productId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal($"Product - {productId} successfully deleted", okResult.Value);
    }

    [Fact]
    public async Task CreateProduct_ReturnsBadRequestIfProductIsNull()
    {
        // Arrange
        Products.Domain.Entities.Product product = null;
        // Arrange
        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateProductCommand>(), default)).ReturnsAsync(0);

        // Act
        var result = await _controller.CreateProduct(product);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Product is empty or null!", badRequestResult.Value);
    }


    [Fact]
    public async Task DeleteProduct_ReturnsOkResult_WithProductIdIsZero()
    {
        var productId = 0;

        // Act
        var result = await _controller.DeleteProduct(productId);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Id is empty or null!", badRequestResult.Value);

    }

}