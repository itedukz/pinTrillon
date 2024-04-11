using AutoMapper;
using ms.MainApi.Entity.Models.DbModels.Products;
using ms.MainApi.Entity.Models.Dtos.Measures;
using ms.MainApi.Entity.Models.Dtos.Products;

namespace ms.MainApi.Entity.Models.Profiles.Products;

public class ProductShortDtoProfile : Profile
{
    public ProductShortDtoProfile()
    {
        CreateMap<Product, ProductShortDto>()
            .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.id))
            .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.name))
            .ForMember(dest => dest.description, opt => opt.MapFrom(src => src.description))
            .ForMember(dest => dest.price, opt => opt.MapFrom(src => src.price))
            .ForMember(dest => dest.measure, opt => opt.MapFrom(src => new MeasureDto(src.height, src.width, src.length, src.measureType)));
    }
}