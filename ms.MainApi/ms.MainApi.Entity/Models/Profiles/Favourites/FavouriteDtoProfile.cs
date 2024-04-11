﻿using AutoMapper;
using ms.MainApi.Entity.Models.DbModels;
using ms.MainApi.Entity.Models.Dtos.Baskets;
using ms.MainApi.Entity.Models.Enums;

namespace ms.MainApi.Entity.Models.Profiles.Favourites;

public class FavouriteDtoProfile : Profile
{
    public FavouriteDtoProfile()
    {
        CreateMap<Basket, BasketDto>()
            .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.id))
            .ForMember(dest => dest.referenceType,
                opt => opt.MapFrom(src => ReferenceTypesMethod.toBaseClass(src.referenceType)));
    }
}