using MediatR;
using Onyx.ProductsApi.Queries;
using Onyx.ProductsService;

namespace Onyx.ProductsApi.Handlers
{
    public class GetProductsByColourQueryHandler : IRequestHandler<GetProductsByColourQuery, List<Products.Domain.Entities.Product>>
    {
        private readonly IProductService _productService;
        public GetProductsByColourQueryHandler(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<List<Products.Domain.Entities.Product>> Handle(GetProductsByColourQuery request, CancellationToken cancellationToken)
        {
            if(string.IsNullOrEmpty(request.Colour))
            {
                throw new ArgumentNullException(nameof(request.Colour));
            }

            return await _productService.GetByColourAsync(request.Colour);
        }
    }
}
