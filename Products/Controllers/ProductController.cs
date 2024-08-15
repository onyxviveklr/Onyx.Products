using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Onyx.Products.Domain.Entities;
using Onyx.ProductsApi.Commands;
using Onyx.ProductsApi.Dto;
using Onyx.ProductsApi.Queries;

namespace Products.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IMapper mapper, IMediator mediator, ILogger<ProductController> logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
           var products = await _mediator.Send(new GetProductsQuery());

           var productsDto = _mapper.Map<IEnumerable<ProductDto>>(products);

            return Ok(productsDto);
        }

        [HttpGet]
        [Route("GetProductsByColour")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByColour(string colour)
        {
            if(string.IsNullOrEmpty(colour))
            {
                return BadRequest("Colour is empty or null!");
            }

            var products = await _mediator.Send(new GetProductsByColourQuery(colour));
            
            var productsDto = _mapper.Map<IEnumerable<ProductDto>>(products);
            
            return Ok(productsDto);
        }

        [HttpPost]
        public async Task<ActionResult> CreateProduct(Product product)
        {
            if(product == null) 
            {
                return BadRequest("Product is empty or null!");
            }

            var productId = await _mediator.Send(new CreateProductCommand { Product = product });
            
            return Ok($"Product - {productId} successfully created!");
        }

        [HttpPut]
        public async Task<ActionResult> UpdateProduct(Product product)
        {
            if (product == null)
            {
                return BadRequest("Product is empty or null!");
            }

            var productId = await _mediator.Send(new UpdateProductCommand(product));
            
            return Ok($"Product - {productId} successfully updated!");
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            if(id == 0) 
            {
                return BadRequest("Id is empty or null!");
            }

            await _mediator.Send(new DeleteProductCommand(id));

            return Ok($"Product - {id} successfully deleted");
        }

    }
}
