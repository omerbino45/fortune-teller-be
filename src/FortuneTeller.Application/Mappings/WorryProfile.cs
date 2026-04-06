using AutoMapper;
using FortuneTeller.Application.DTOs;
using FortuneTeller.Domain.Entities;

namespace FortuneTeller.Application.Mappings;

public class WorryProfile : Profile
{
    public WorryProfile()
    {
        CreateMap<Worry, WorryResponse>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
    }
}
