using MediatR;
using Onyx.ProductsApi.Queries;
using Onyx.ProductsService;

namespace Onyx.ProductsApi.Handlers
{
    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, List<Products.Domain.Entities.Product>>
    {
        private readonly IProductService _productService;
        public GetProductsQueryHandler(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<List<Products.Domain.Entities.Product>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            return await _productService.GetAllAsync();
        }
    }
}
