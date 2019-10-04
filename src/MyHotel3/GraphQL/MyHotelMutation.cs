using AutoMapper;
using GraphQL.Types;
using MyHotel.EntityFrameworkCore.Entities;
using MyHotel.GraphQL.Types;
using MyHotel.Models;
using MyHotel.Repositories;
using MyHotel.Services;

namespace MyHotel.GraphQL
{
    public class MyHotelMutation : ObjectGraphType
    {
        public MyHotelMutation(MyHotelRepository myHotelRepository, INotifier notifier, IMapper mapper)
        {
            Name = "CreateGuestMutation";

            Field<GuestType>(
                "createGuest",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<GuestInputType>> { Name = "guest" }
                ),
                resolve: context =>
                {
                    var guestModel = context.GetArgument<GuestModel>("guest");
                    var guest = mapper.Map<Guest>(guestModel);
                    var newGuest = myHotelRepository.AddGuest(guest);
                    var newGuestModel = mapper.Map<GuestModel>(newGuest);

                    notifier.AddGuest(newGuestModel);

                    return newGuestModel;
                });
        }
    }
}