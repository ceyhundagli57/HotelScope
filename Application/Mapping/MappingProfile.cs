using Application.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Hotel mappings
        CreateMap<HotelEntity, HotelDto>().ReverseMap();
        CreateMap<HotelEntity, CreateHotelDto>().ReverseMap();
        CreateMap<HotelEntity, UpdateHotelDto>().ReverseMap();

        // Contact mappings
        CreateMap<HotelContactInfoEntity, HotelContactInfoDto>().ReverseMap();
        CreateMap<HotelContactInfoEntity, CreateContactInfoDto>().ReverseMap();
        CreateMap<HotelContactInfoEntity, UpdateContactInfoDto>().ReverseMap();

        // Staff mappings
        CreateMap<HotelStaffEntity, HotelStaffDto>().ReverseMap();
        CreateMap<HotelStaffEntity, CreateStaffDto>().ReverseMap();
        CreateMap<HotelStaffEntity, UpdateStaffDto>().ReverseMap();
            
        //Report mappings
        CreateMap<ReportEntity, ReportDto>().ReverseMap();
        CreateMap<ReportEntity, ReportStatusDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
        CreateMap<ReportEntity, ReportSummaryDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
    }
}