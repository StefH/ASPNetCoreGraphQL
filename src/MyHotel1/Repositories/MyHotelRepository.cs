using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using MyHotel.Entities;
using MyHotel.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyHotel.Repositories
{
    public class MyHotelRepository
    {
        private readonly MyHotelDbContext _myHotelDbContext;

        public MyHotelRepository(MyHotelDbContext myHotelDbContext)
        {
            _myHotelDbContext = myHotelDbContext;
        }

        public async Task<List<Reservation>> GetAll()
        {
            return await GetReservationsQuery().AsQueryable().ToListAsync();
        }

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

        public IQueryable<Room> GetRoomsQuery()
        {
            return _myHotelDbContext.Rooms;
        }

        public IIncludableQueryable<Room, RoomDetail> GetRoomsQuery2()
        {
            return _myHotelDbContext.Rooms.Include(x => x.RoomDetail);
        }
    }
}