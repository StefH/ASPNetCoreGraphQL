using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Text.RegularExpressions;
using GraphQL.EntityFrameworkCore.DynamicLinq.Enumerations;
using GraphQL.EntityFrameworkCore.DynamicLinq.Models;
using GraphQL.EntityFrameworkCore.DynamicLinq.Validation;
using GraphQL.Types;
using JetBrains.Annotations;

namespace GraphQL.EntityFrameworkCore.DynamicLinq.Extensions
{
    public static class QueryableExtensions
    {
        private const string SortOrderAsc = "asc";
        private const string SortOrderDesc = "desc";
        private static readonly Regex OrderByRegularExpression = new Regex(@"\w+", RegexOptions.Compiled);

        [PublicAPI]
        public static IQueryable<T> ApplyQueryArguments<T>([NotNull] this IQueryable<T> query, [NotNull] QueryArgumentInfoList list, [CanBeNull] Dictionary<string, object> arguments)
        {
            Guard.NotNull(query, nameof(query));
            Guard.HasNoNulls(list, nameof(list));

            var orderByEntityPropertyPaths = new List<(string value, bool isEntityPath)>();
            if (arguments != null)
            {
                foreach (var argument in arguments)
                {
                    var filterQueryArgumentInfo = list.FirstOrDefault(i => i.QueryArgumentInfoType == QueryArgumentInfoType.DefaultGraphQL && i.QueryArgument.Name == argument.Key);
                    if (filterQueryArgumentInfo != null)
                    {
                        query = query.Where(BuildPredicate(filterQueryArgumentInfo, argument.Value));
                    }

                    var orderByQueryArgumentInfo = list.FirstOrDefault(i => i.QueryArgumentInfoType == QueryArgumentInfoType.OrderBy && i.QueryArgument.Name == argument.Key);
                    if (orderByQueryArgumentInfo != null && argument.Value is string orderByStatement)
                    {
                        foreach (Match match in OrderByRegularExpression.Matches(orderByStatement))
                        {
                            if (match.Value.Equals(SortOrderAsc, StringComparison.OrdinalIgnoreCase) || match.Value.Equals(SortOrderDesc, StringComparison.OrdinalIgnoreCase))
                            {
                                orderByEntityPropertyPaths.Add((match.Value.ToLower(), false));
                            }

                            var queryArgumentInfo = list.FirstOrDefault(l => match.Value.Equals(l.GraphQLPath, StringComparison.OrdinalIgnoreCase));
                            if (queryArgumentInfo != null)
                            {
                                orderByEntityPropertyPaths.Add((queryArgumentInfo.EntityPath, true));
                            }
                        }
                    }
                }
            }

            if (orderByEntityPropertyPaths.Any())
            {
                var stringBuilder = new StringBuilder();
                foreach (var orderByEntityPropertyPath in orderByEntityPropertyPaths)
                {
                    stringBuilder.AppendFormat("{0}{1}", orderByEntityPropertyPath.isEntityPath ? ',' : ' ', orderByEntityPropertyPath.value);
                }

                query = query.OrderBy(stringBuilder.ToString().TrimStart(','));
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