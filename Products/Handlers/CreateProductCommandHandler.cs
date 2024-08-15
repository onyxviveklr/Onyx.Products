using MediatR;
using Onyx.ProductsApi.Commands;
using Onyx.ProductsService;

namespace Onyx.ProductsApi.Handlers
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
    {
        private readonly IProductService _productService;
        public CreateProductCommandHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            if(request.Product == null)
            {
                throw new ArgumentNullException($"{nameof(CreateProductCommandHandler)} - Product is empty or null!");
            }

            return await _productService.AddProductAsync(request.Product);
        }
    }
}
