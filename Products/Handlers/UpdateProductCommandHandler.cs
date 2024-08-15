using MediatR;
using Onyx.ProductsApi.Commands;
using Onyx.ProductsService;

namespace Onyx.ProductsApi.Handlers
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Products.Domain.Entities.Product>
    {
        private readonly IProductService _productService;

        public UpdateProductCommandHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<Products.Domain.Entities.Product> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            if(request.Product == null)
            {
                throw new ArgumentNullException($"{ nameof(request.Product) } is null");
            }

            return await _productService.UpdateProductAsync(request.Product);
        }
    }
}
