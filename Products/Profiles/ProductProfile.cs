using AutoMapper;
using Onyx.ProductsApi.Dto;

namespace Onyx.ProductsApi.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Onyx.Products.Domain.Entities.Product, ProductDto>()
                .ForMember(dest => dest.ModelName, opt => opt.MapFrom(src => src.Model.ModelName))
                .ForMember(dest => dest.PriceAmount, opt => opt.MapFrom(src => src.Price.Amount))
                .ForMember(dest => dest.ColourName, opt => opt.MapFrom(src => src.Colour.ColourName));
        }
    }
}
