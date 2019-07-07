using System;
using GraphQL.Resolvers;
using GraphQL.Types;
using NHLStats.Api.Helpers;

namespace NHLStats.Api.Models
{
    public class NHLStatsQuery : ObjectGraphType
    {
        public NHLStatsQuery(ContextServiceLocator contextServiceLocator)
        {
            Field<PlayerType>(
                "player",
                arguments: new QueryArguments(new QueryArgument<IntGraphType> { Name = "id" }),
                resolve: context => contextServiceLocator.PlayerRepository.Get(context.GetArgument<int>("id")));

            Field<PlayerType>(
                "playerByName",
                arguments: new QueryArguments(new QueryArgument<StringGraphType> { Name = "name" }),
                resolve: context => contextServiceLocator.PlayerRepository.GetByName(context.GetArgument<string>("name")));

            Field<PlayerType>(
                "randomPlayer",
                resolve: context => contextServiceLocator.PlayerRepository.GetRandom());

            Field<ListGraphType<PlayerType>>(
                "players",
                resolve: context => contextServiceLocator.PlayerRepository.All());

            //Field<PlayerType>(
            //    "playerByBirthPlace",
            //    arguments: new QueryArguments(new QueryArgument<ObjectGraphType> { Name = "x" }),
            //    resolve: context => contextServiceLocator.PlayerRepository.GetByDynamic(context.GetArgument<object>("x")));

            //AddField(new FieldType
            //{
            //    Name = "playerByBirthPlace",
            //    Arguments = new QueryArguments(new QueryArgument<StringGraphType> { Name = "birthPlace" }),
            //    Resolver = new NameFieldResolver()
            //});
        }

        public class NameFieldResolver : IFieldResolver
        {
            public object Resolve(ResolveFieldContext context)
            {
                var source = context.Source;

                if (source == null)
                {
                    return null;
                }

                string name = char.ToUpperInvariant(context.FieldAst.Name[0]) + context.FieldAst.Name.Substring(1);
                var value = GetPropValue(source, name);

                if (value == null)
                {
                    throw new InvalidOperationException($"Expected to find property {context.FieldAst.Name} on {context.Source.GetType().Name} but it does not exist.");
                }

                return value;
            }

            private static object GetPropValue(object src, string propName)
            {
                return src.GetType().GetProperty(propName).GetValue(src, null);
            }
        }
    }
}