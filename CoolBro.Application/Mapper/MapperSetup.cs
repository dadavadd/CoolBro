using AutoMapper;
using CoolBro.Application.DTOs;
using CoolBro.Domain.Entities;

namespace CoolBro.Application.Mapper;

public class MapperSetup : Profile
{
    public MapperSetup()
    {
        CreateMap<User, AdminDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.TelegramId, opt => opt.MapFrom(src => src.TelegramId))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role));
    }
}
