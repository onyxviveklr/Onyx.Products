using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json;
using Onyx.Products.Domain.Entities;
using Onyx.ProductsApi.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Onyx.Products.Tests.IntegrationTests;

public class ProductControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private Products.Domain.Entities.Product product;
    private string token;

    public ProductControllerIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
      //  _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

        product = new Products.Domain.Entities.Product
        {
            Id = 1,
            Name = "Product1",
            Model = new ProductModel { Id = 1, ModelName = "Test" },
            Colour = new ProductColour { Id = 1, ColourName = "Red", },
            Price = new ProductPrice { Id = 1, Amount = 300 }
        };


    }

    [Fact]
    public async Task GetProducts_ReturnsOkResult_WithListOfProducts()
    {
        //Arramge
        await AddAuthHeader();

        // Act
        var response = await _client.GetAsync("/api/Product");
        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();
        var products = JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(responseString);
        Assert.NotNull(products);
    }



    [Fact]
    public async Task GetProductsByColour_ReturnsBadRequest_WhenColourIsNullOrEmpty()
    {
        //Arramge
        await AddAuthHeader();

        // Act
        var response = await _client.GetAsync("/api/Product/GetProductsByColour?colour=");

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CreateProduct_ReturnsOkResult_WithProduct()
    {
        //Arramge
        product = new Products.Domain.Entities.Product
        {
            Name = "Product1",
            Model = new ProductModel { ModelName = "Test" },
            Colour = new ProductColour { ColourName = "Red" },
            Price = new ProductPrice {  Amount = 300 }
        };

        await AddAuthHeader();
        var content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/Product", content);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        Assert.Contains("successfully created", responseString);
    }


    [Fact]
    public async Task DeleteProduct_ReturnsBadRequest_WhenIdIsZero()
    {
        //Arramge
        await AddAuthHeader();

        // Act
        var response = await _client.DeleteAsync("/api/Product?id=0");

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var responseString = await response.Content.ReadAsStringAsync();
        Assert.Equal("Id is empty or null!", responseString);
    }
    private async Task AddAuthHeader()
    {
        var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", new
        {
            Username = "Onyxuser",
            Password = "OnyxPassw0rd"
        });

        var jsonResponse = await loginResponse.Content.ReadAsStringAsync();
        var jsonDocument = JsonDocument.Parse(jsonResponse);
        var token = jsonDocument.RootElement.GetProperty("token").GetString();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}