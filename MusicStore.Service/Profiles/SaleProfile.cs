using AutoMapper;
using MusicStore.Dto.Response;
using MusicStore.Entities;

namespace MusicStore.Service.Profiles;

public class SaleProfile : Profile
{
    public SaleProfile()
    {
        CreateMap<Sale, SaleDtoResponse>()
            .ForMember(des => des.SaleId, opt => opt.MapFrom(src => src.Id))
            .ForMember(des => des.DateEvent, opt => opt.MapFrom(src => src.Concert.DateEvent.ToString("yyyy-MM-dd")))
            .ForMember(des => des.TimeEvent, opt => opt.MapFrom(src => src.Concert.DateEvent.ToString("HH:mm:")))
            .ForMember(des => des.Genre, opt => opt.MapFrom(src => src.Concert.Genre.Name))
            .ForMember(des => des.ImageUrl, opt => opt.MapFrom(src => src.Concert.ImageUrl))
            .ForMember(des => des.Title, opt => opt.MapFrom(src => src.Concert.Title))
            .ForMember(des => des.OperationNumber, opt => opt.MapFrom(src => src.OperationNumber))
            .ForMember(des => des.FullName, opt => opt.MapFrom(src => src.Customer.FullName))
            .ForMember(des => des.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ForMember(des => des.SaleDate, opt => opt.MapFrom(src => src.SaleDate.ToString("yyyy-MM-dd HH:mm:ss")))
            .ForMember(des => des.Total, opt => opt.MapFrom(src => src.Total));

        //todo: minuto 14
    }
}