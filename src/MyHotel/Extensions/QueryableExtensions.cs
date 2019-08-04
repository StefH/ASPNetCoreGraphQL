using GraphQL.Types;
using MyHotel.GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace MyHotel.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable ApplyQueryArguments(this IQueryable query, ICollection<QueryArgumentInfo> list, Dictionary<string, object> arguments)
        {
            if (arguments != null)
            {
                foreach (var argument in arguments)
                {
                    var info = list.FirstOrDefault(i => i.QueryArgument.Name == argument.Key);
                    if (info != null)
                    {
                        query = query.Where(GetWhere(info, argument.Value));
                    }
                }
            }

            return query;
        }

        private static string GetWhere(QueryArgumentInfo info, object value)
        {
            if (info.QueryArgument.Type == typeof(DateGraphType) && value is DateTime date)
            {
                return $"np({info.EntityPropertyPath}) >= \"{date}\" && np({info.EntityPropertyPath}) < \"{date.AddDays(1)}\"";
            }

            return $"np({info.EntityPropertyPath}) == \"{value}\"";
        }
    }
}