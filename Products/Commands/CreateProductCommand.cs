using MediatR;

namespace Onyx.ProductsApi.Commands
{
    public class CreateProductCommand : IRequest<int>
    {
        public Products.Domain.Entities.Product Product { get; set; }
    }
}
