using AutoMapper;
using MyHotel.Entities;
using MyHotel.Models;

namespace MyHotel.AutoMapper
{
    public class MyHotelProfile : Profile
    {
        public MyHotelProfile()
        {
            CreateMap<Guest, GuestModel>();
            CreateMap<Room, RoomModel>()
                .ForMember(m => m.Detail, opt => opt.MapFrom(e => e.RoomDetail));
            CreateMap<RoomDetail, RoomDetailModel>()
                .ForMember(m => m.Identifier, opt => opt.MapFrom(e => e.Id));
            CreateMap<Reservation, ReservationModel>();
        }
    }
}