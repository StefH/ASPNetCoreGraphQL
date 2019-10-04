using System;
using System.Collections.Concurrent;
using MyHotel.Models;

namespace MyHotel.Services
{
    public interface INotifier
    {
        // ConcurrentStack<GuestModel> AllGuests { get; }

        IObservable<GuestModel> Guest();

        void AddGuest(GuestModel guest);
    }
}