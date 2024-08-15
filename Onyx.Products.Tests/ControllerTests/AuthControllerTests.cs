using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Onyx.ProductsApi.Controllers;
using Onyx.ProductsApi.Models;


namespace Onyx.Products.Tests.ControllerTests;

public class AuthControllerTests
{
    private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.Test.json", optional: false, reloadOnChange: true)
        .Build();

        _controller = new AuthController(_configuration);
    }

    [Fact]
    public void Login_ReturnsUnauthorized_WhenUserLoginIsNull()
    {
        // Act
        var result = _controller.Login(null);

        // Assert
        var unauthorizedResult = Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public void Login_ReturnsUnauthorized_WhenCredentialsAreInvalid()
    {
        // Arrange
        var userLogin = new UserLogin { Username = "invaliduser", Password = "invalidpassword" };

        // Act
        var result = _controller.Login(userLogin);

        // Assert
        var unauthorizedResult = Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public void Login_ReturnsOk_WithToken_WhenCredentialsAreValid()
    {
        // Arrange
        var userLogin = new UserLogin { Username = "Onyxuser", Password = "OnyxPassw0rd" };

        // Act
        var result = _controller.Login(userLogin);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var token = okResult.Value;
        Assert.NotNull(token);
    }
}