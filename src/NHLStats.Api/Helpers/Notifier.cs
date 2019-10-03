using System;
using System.Collections.Concurrent;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using NHLStats.Core.Models;

namespace NHLStats.Api.Helpers
{
    // based on https://github.com/graphql-dotnet/server/blob/develop/samples/Samples.Schemas.Chat/IChat.cs
    public class Notifier : INotifier
    {
        private readonly ISubject<Player> _playerStream = new ReplaySubject<Player>(1);

        public ConcurrentStack<Player> AllPlayers { get; }

        public Notifier()
        {
            AllPlayers = new ConcurrentStack<Player>();
        }

        public void AddPlayer(Player player)
        {
            AllPlayers.Push(player);
            _playerStream.OnNext(player);
        }

        public IObservable<Player> Player()
        {
            return _playerStream
                .Select(player => player)
                .AsObservable();
        }
    }
}