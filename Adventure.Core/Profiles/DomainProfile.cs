using AutoMapper;

namespace Adventure.Core.Profiles;

public class DomainProfile : Profile
{
    public DomainProfile()
    {
        CreateMap<Dto.AdventureReadModel, Domain.Adventure>()
            .ForMember(dest => dest.Routes, m => m.MapFrom(src => src.Routes))
            .ReverseMap();

        CreateMap<Dto.Adventure, Domain.Adventure>()
            .ForMember(dest => dest.Routes, m => m.MapFrom(src => src.Routes))
            .ReverseMap();

        CreateMap<Dto.User, Domain.User>().ReverseMap();

        CreateMap<Dto.UserAdventure, Domain.UserAdventureAggregate>()
            .ForMember(dest => dest.Adventure, m => m.MapFrom(src => src.Adventure))
            .ForMember(dest => dest.User, m => m.MapFrom(src => src.User))
            .ForMember(dest => dest.SelectedRoutes, m => m.MapFrom(src => src.SelectedRoutes))
            .ReverseMap();
        
        CreateMap<Dto.SelectedRoute, Domain.ValueTypes.SelectedRoute>().ReverseMap();

        CreateMap<Dto.Route, Domain.ValueTypes.Route>().ReverseMap();
    }
}