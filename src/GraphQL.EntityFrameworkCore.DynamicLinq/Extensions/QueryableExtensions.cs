using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using GraphQL.EntityFrameworkCore.DynamicLinq.Models;
using GraphQL.EntityFrameworkCore.DynamicLinq.Validation;
using GraphQL.Types;
using JetBrains.Annotations;

namespace GraphQL.EntityFrameworkCore.DynamicLinq.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> ApplyQueryArguments<T>([NotNull] this IQueryable<T> query, [NotNull] QueryArgumentInfoList list, [CanBeNull] Dictionary<string, object> arguments)
        {
            Guard.NotNull(query, nameof(query));
            Guard.HasNoNulls(list, nameof(list));

            if (arguments != null)
            {
                foreach (var argument in arguments)
                {
                    var info = list.FirstOrDefault(i => i.QueryArgument.Name == argument.Key);
                    if (info != null)
                    {
                        query = query.Where(BuildPredicate(info, argument.Value));
                    }
                }
            }

            return query;
        }

        private static string BuildPredicate(QueryArgumentInfo info, object value)
        {
            if (info.QueryArgument.Type == typeof(DateGraphType) && value is DateTime date)
            {
                return $"np({info.EntityPath}) >= \"{date}\" && np({info.EntityPath}) < \"{date.AddDays(1)}\"";
            }

            return $"np({info.EntityPath}) == \"{value}\"";
        }
    }
}