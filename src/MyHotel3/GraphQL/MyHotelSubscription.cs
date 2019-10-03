using System;
using GraphQL.Resolvers;
using GraphQL.Subscription;
using GraphQL.Types;
using MyHotel.GraphQL.Types;
using MyHotel.Models;
using MyHotel.Services;

namespace MyHotel.GraphQL
{
    public class MyHotelSubscription : ObjectGraphType
    {
        private readonly INotifier _notifier;

        public MyHotelSubscription(INotifier notifier)
        {
            _notifier = notifier;

            AddField(new EventStreamFieldType
            {
                Name = "guestAdded",
                Type = typeof(GuestType),
                Resolver = new FuncFieldResolver<GuestModel>(ResolveGuest),
                Subscriber = new EventStreamResolver<GuestModel>(Subscribe)
            });
        }

        private GuestModel ResolveGuest(ResolveFieldContext context)
        {
            return context.Source as GuestModel;
        }

        private IObservable<GuestModel> Subscribe(ResolveEventStreamContext context)
        {
            return _notifier.Guest();
        }
    }
}
