using AutoMapper;
using ms.MainApi.Entity.Models.DbModels.Projects;
using ms.MainApi.Entity.Models.Dtos.Projects;

namespace ms.MainApi.Entity.Models.Profiles.Projects;

public class ProjectCreateProfile : Profile
{
    public ProjectCreateProfile()
    {
        CreateMap<ProjectCreateDto, Project>()
            .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.name))
            .ForMember(dest => dest.description, opt => opt.MapFrom(src => src.description))
            .ForMember(dest => dest.price, opt => opt.MapFrom(src => src.price))
            .ForMember(dest => dest.height, opt => opt.MapFrom(src => src.measure!.height))
            .ForMember(dest => dest.width, opt => opt.MapFrom(src => src.measure!.width))
            .ForMember(dest => dest.length, opt => opt.MapFrom(src => src.measure!.length));
    }
}