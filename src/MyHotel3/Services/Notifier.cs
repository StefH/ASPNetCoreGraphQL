using System;
using System.Collections.Concurrent;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using MyHotel.Models;

namespace MyHotel.Services
{
    // based on https://github.com/graphql-dotnet/server/blob/develop/samples/Samples.Schemas.Chat/IChat.cs
    public class Notifier : INotifier
    {
        private readonly ISubject<GuestModel> _guestStream = new ReplaySubject<GuestModel>(1);

        public ConcurrentStack<GuestModel> AllGuests { get; }

        public Notifier()
        {
            AllGuests = new ConcurrentStack<GuestModel>();
        }

        public void AddGuest(GuestModel player)
        {
            AllGuests.Push(player);
            _guestStream.OnNext(player);
        }

        public IObservable<GuestModel> Guest()
        {
            return _guestStream
                .Select(player => player)
                .AsObservable();
        }
    }
}