using MediatR;

namespace Onyx.ProductsApi.Queries
{
    public class GetProductsQuery : IRequest<List<Products.Domain.Entities.Product>>
    {
        public Products.Domain.Entities.Product Product { get; set; }
    }
}
