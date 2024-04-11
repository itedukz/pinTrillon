using AutoMapper;
using ms.MainApi.Entity.Models.DbModels.Products;
using ms.MainApi.Entity.Models.Dtos.Products;

namespace ms.MainApi.Entity.Models.Profiles.Products;

public class ProductUpdateProfile : Profile
{
	public ProductUpdateProfile()
	{
        CreateMap<ProductUpdateDto, Product>()
            .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.id))
            .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.name))
            .ForMember(dest => dest.description, opt => opt.MapFrom(src => src.description))
            .ForMember(dest => dest.price, opt => opt.MapFrom(src => src.price))
            .ForMember(dest => dest.productArticleId, opt => opt.MapFrom(src => src.productArticleId))
            .ForMember(dest => dest.brandId, opt => opt.MapFrom(src => src.brandId))
            .ForMember(dest => dest.materialsId, opt => opt.MapFrom(src => src.materialsId))
            .ForMember(dest => dest.height, opt => opt.MapFrom(src => src.measure!.height))
            .ForMember(dest => dest.width, opt => opt.MapFrom(src => src.measure!.width))
            .ForMember(dest => dest.length, opt => opt.MapFrom(src => src.measure!.length))
            .ForMember(dest => dest.measureType, opt => opt.MapFrom(src => src.measure!.measureType));
    }
}