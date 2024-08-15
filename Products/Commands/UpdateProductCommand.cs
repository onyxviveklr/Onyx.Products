using MediatR;

namespace Onyx.ProductsApi.Commands
{
    public class UpdateProductCommand : IRequest<Products.Domain.Entities.Product>
    {
        public UpdateProductCommand(Products.Domain.Entities.Product product)
        {
            Product = product;
        }
        public Products.Domain.Entities.Product Product { get; set; }
    }
}
