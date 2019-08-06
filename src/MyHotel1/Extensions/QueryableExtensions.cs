using GraphQL.Language.AST;
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
        public static IQueryable ApplyQueryArguments<T>(this IQueryable<T> query, ICollection<QueryArgumentInfo> list, Dictionary<string, object> arguments, IDictionary<string, Field> fields)
        {
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

            if (fields != null)
            {
                // var entityPropertyPaths = PopulateSelection(list, fields);
                // query = query.Select()
            }

            return query;
        }

        private static string BuildPredicate(QueryArgumentInfo info, object value)
        {
            if (info.QueryArgument.Type == typeof(DateGraphType) && value is DateTime date)
            {
                return $"np({info.EntityPropertyPath}) >= \"{date}\" && np({info.EntityPropertyPath}) < \"{date.AddDays(1)}\"";
            }

            return $"np({info.EntityPropertyPath}) == \"{value}\"";
        }

        private static ICollection<string> PopulateSelection(ICollection<QueryArgumentInfo> list, IDictionary<string, Field> fields)
        {
            var entityPropertyPaths = new List<string>();
            foreach (var key in fields.Keys)
            {
                if (!fields[key].SelectionSet.Selections.Any())
                {
                    var x = list.FirstOrDefault(q => string.Compare(q.GraphPropertyPath, key, StringComparison.OrdinalIgnoreCase) == 0);
                    if (x != null)
                    {
                        entityPropertyPaths.Add(x.EntityPropertyPath);
                    }
                }
                else
                {
                    int f = 0;
                    // entityPropertyPaths.AddRange(PopulateSelection(list, fields[key].SelectionSet.Children));
                }
            }

            return entityPropertyPaths;
        }

        private static string BuildSelection(QueryArgumentInfo info)
        {
            return null;
        }
    }
}