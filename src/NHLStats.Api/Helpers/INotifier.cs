using System;
using System.Collections.Concurrent;
using NHLStats.Core.Models;

namespace NHLStats.Api.Helpers
{
    public interface INotifier
    {
        ConcurrentStack<Player> AllPlayers { get; }

        IObservable<Player> Player();

        void AddPlayer(Player player);
    }
}