using AutoMapper;
using ms.MainApi.Entity.Models.DbModels.Projects;
using ms.MainApi.Entity.Models.Dtos.Measures;
using ms.MainApi.Entity.Models.Dtos.Projects;
using ms.MainApi.Entity.Models.Enums;

namespace ms.MainApi.Entity.Models.Profiles.Projects;

public class ProjectShortDtoProfile : Profile
{
    public ProjectShortDtoProfile()
    {
        CreateMap<Project, ProjectShortDto>()
            .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.id))
            .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.name))
            //.ForMember(dest => dest.article, opt => opt.MapFrom(src => src.article))
            .ForMember(dest => dest.description, opt => opt.MapFrom(src => src.description))
            .ForMember(dest => dest.price, opt => opt.MapFrom(src => src.price))
            .ForMember(dest => dest.quadrature, opt => opt.MapFrom(src => src.quadrature))
            .ForMember(dest => dest.measure, opt => opt.MapFrom(src => new MeasureDto(src.height, src.width, src.length, (int)MeasureEnum.m)));
    }
}