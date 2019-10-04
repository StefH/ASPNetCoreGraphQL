using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using MyHotel.Models;

namespace MyHotel.Services
{
    // based on
    // - https://github.com/graphql-dotnet/server/blob/develop/samples/Samples.Schemas.Chat/IChat.cs
    // - https://github.com/putridparrot/blog-projects/blob/master/GraphQLService/GraphQLService/PersonSubscription.cs
    public class Notifier : INotifier
    {
        private readonly ISubject<GuestModel> _guestStream = new ReplaySubject<GuestModel>(1);

        public void AddGuest(GuestModel guest)
        {
            // AllGuests.Push(guest);
            _guestStream.OnNext(guest);
        }

        public IObservable<GuestModel> Guest()
        {
            return _guestStream
                //.Select(guest => guest)
                .AsObservable();

            // return Observable.Interval(TimeSpan.FromSeconds(1)).Select(s => new GuestModel { Name = s.ToString() });
        }
    }
}