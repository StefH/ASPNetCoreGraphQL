using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using MyHotel.Entities;
using MyHotel.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace MyHotel.Repositories
{
    public class ReservationRepository
    {
        private readonly MyHotelDbContext _myHotelDbContext;
        private readonly IMapper _mapper;

        public ReservationRepository(MyHotelDbContext myHotelDbContext, IMapper mapper)
        {
            _myHotelDbContext = myHotelDbContext;
            _mapper = mapper;
        }

        public async Task<List<T>> GetAll<T>()
        {
            return await GetReservationsQuery().ProjectTo<T>(_mapper.ConfigurationProvider).ToListAsync();
        }

        //public async Task<IEnumerable<Reservation>> GetAll()
        //{
        //    return await _myHotelDbContext
        //        .Reservations
        //        .Include(x => x.Room)
        //        .Include(x => x.Room.RoomDetail)
        //        .Include(x => x.Guest)
        //        .ToListAsync();
        //}

        public Reservation Get(int id)
        {
            return GetReservationsQuery().Single(x => x.Id == id);
        }

        public IIncludableQueryable<Reservation, Guest> GetReservationsQuery()
        {
            return _myHotelDbContext
                .Reservations
                .Include(x => x.Room)
                .Include(x => x.Room.RoomDetail)
                .Include(x => x.Guest);
        }

        public IIncludableQueryable<Room, RoomDetail> GetRoomsQuery()
        {
            return _myHotelDbContext.Rooms
                .Include(x => x.RoomDetail);
        }
    }
}
