using GraphQL.Types;
using NHLStats.Api.Helpers;
using NHLStats.Core.Models;

namespace NHLStats.Api.Models
{
    public class NHLStatsMutation : ObjectGraphType
    {
        public NHLStatsMutation(ContextServiceLocator contextServiceLocator)
        {
            Name = "CreatePlayerMutation";

            Field<PlayerType>(
                "createPlayer",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<PlayerInputType>> { Name = "player" }
                ),
                resolve: context =>
                {
                    var player = context.GetArgument<Player>("player");
                    var newPlayer = contextServiceLocator.PlayerRepository.Add(player);

                    contextServiceLocator.Notifier.AddPlayer(player);

                    return newPlayer;
                });
        }
    }
}