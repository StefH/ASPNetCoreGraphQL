using System;
using GraphQL.Resolvers;
using GraphQL.Subscription;
using GraphQL.Types;
using NHLStats.Api.Helpers;
using NHLStats.Core.Models;

namespace NHLStats.Api.Models
{
    public class NHLStatsSubscription : ObjectGraphType
    {
        private readonly INotifier _notifier;

        public NHLStatsSubscription(INotifier notifier)
        {
            _notifier = notifier;

            AddField(new EventStreamFieldType
            {
                Name = "playerAdded",
                Type = typeof(PlayerType),
                Resolver = new FuncFieldResolver<Player>(ResolvePlayer),
                Subscriber = new EventStreamResolver<Player>(Subscribe)
            });
        }

        private Player ResolvePlayer(ResolveFieldContext context)
        {
            return context.Source as Player;
        }

        private IObservable<Player> Subscribe(ResolveEventStreamContext context)
        {
            return _notifier.Player();
        }
    }
}
