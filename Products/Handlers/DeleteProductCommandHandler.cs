using MediatR;
using Onyx.ProductsApi.Commands;
using Onyx.ProductsService;

namespace Onyx.ProductsApi.Handlers
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
    {
        private readonly IProductService _productService;
        public DeleteProductCommandHandler(IProductService productService)
        {
            _productService = productService;
        }
        public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            if(request.Id == 0)
            {
                throw new ArgumentException($"{nameof(DeleteProductCommandHandler)} - Id field is 0 or empty!");
            }

            await _productService.DeleteProductAsync(request.Id);
        }
    }
}
