using MediatR;

namespace Onyx.ProductsApi.Queries
{
    public class GetProductsByColourQuery : IRequest<List<Products.Domain.Entities.Product>>
    {
        public string Colour { get; set; }

        public GetProductsByColourQuery(string colour)
        {
            Colour = colour;
        }
    }
}
