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

        private static bool IsSortOrder(string value) =>
            value != null && (value.Equals(SortOrderAsc, StringComparison.OrdinalIgnoreCase) || value.Equals(SortOrderDesc, StringComparison.OrdinalIgnoreCase));

        [PublicAPI]
        public static IQueryable<T> ApplyQueryArguments<T>([NotNull] this IQueryable<T> query, [NotNull] QueryArgumentInfoList list, [NotNull] ResolveFieldContext<T> context)
        {
            Guard.NotNull(query, nameof(query));
            Guard.HasNoNulls(list, nameof(list));
            Guard.NotNull(context, nameof(context));

            if (context.Arguments == null)
            {
                return query;
            }

            var orderByItems = new List<(string value, QueryArgumentInfoType type, int relatedId)>();

            foreach (var argument in context.Arguments)
            {
                if (TryGetOrderBy(argument.Key, argument.Value, list, context, out string orderByStatement))
                {
                    ApplyOrderBy(list, orderByItems, orderByStatement);
                }
                else
                {
                    var filterQueryArgumentInfo = GetEntityPath(argument.Key, list, context);
                    query = query.Where(BuildPredicate(filterQueryArgumentInfo, argument.Value));
                }
            }

            if (orderByItems.Any())
            {
                var stringBuilder = new StringBuilder();
                foreach (var orderByItem in orderByItems)
                {
                    stringBuilder.AppendFormat("{0}{1}", orderByItem.type == QueryArgumentInfoType.DefaultGraphQL ? ',' : ' ', orderByItem.value);
                }

                query = query.OrderBy(stringBuilder.ToString().TrimStart(','));
            }

            return query;
        }

        private static void ApplyOrderBy(QueryArgumentInfoList list, List<(string value, QueryArgumentInfoType type, int relatedId)> orderByItems, string orderByStatement)
        {
            int index = 0;
            foreach (Match match in OrderByRegularExpression.Matches(orderByStatement))
            {
                if (IsSortOrder(match.Value))
                {
                    int orderByIndex = index - 1;
                    if (orderByItems.Count(o => o.type == QueryArgumentInfoType.DefaultGraphQL && o.relatedId == orderByIndex) == 0)
                    {
                        throw new ArgumentException($"The \"{QueryArgumentInfoType.OrderBy}\" field with value \"{match.Value}\" cannot be used without a query field.");
                    }

                    orderByItems.Add((match.Value.ToLower(), QueryArgumentInfoType.OrderBy, orderByIndex));
                }
                else
                {
                    var queryArgumentInfo = list.FirstOrDefault(l => match.Value.Equals(l.GraphQLPath, StringComparison.OrdinalIgnoreCase));
                    if (queryArgumentInfo == null)
                    {
                        throw new ArgumentException($"The \"{QueryArgumentInfoType.OrderBy}\" field uses an unknown field \"{match.Value}\".");
                    }

                    orderByItems.Add((queryArgumentInfo.EntityPath, QueryArgumentInfoType.DefaultGraphQL, index));
                }

                index++;
            }
        }

        private static QueryArgumentInfo GetEntityPath<T>(string argumentName, QueryArgumentInfoList list, ResolveFieldContext<T> context)
        {
            var queryArgumentInfo = list.FirstOrDefault(i => i.QueryArgumentInfoType == QueryArgumentInfoType.DefaultGraphQL && i.QueryArgument.Name == argumentName);
            if (queryArgumentInfo != null)
            {
                return queryArgumentInfo;
            }

            throw new ArgumentException($"Unknown argument \"{argumentName}\" on field \"{context.FieldName}\" of type \"{context.Operation.Name}\"."); // todo
        }

        private static bool TryGetOrderBy<T>(string argumentName, object argumentValue, QueryArgumentInfoList list, ResolveFieldContext<T> context, out string orderByStatement)
        {
            orderByStatement = null;
            var orderByQueryArgumentInfo = list.FirstOrDefault(i => i.QueryArgumentInfoType == QueryArgumentInfoType.OrderBy && i.QueryArgument.Name == argumentName);
            if (orderByQueryArgumentInfo != null && argumentValue is string orderByAsString)
            {
                if (string.IsNullOrWhiteSpace(orderByAsString))
                {
                    throw new ArgumentException($"The \"{QueryArgumentInfoType.OrderBy}\" field is empty.");
                }

                orderByStatement = orderByAsString;
                return true;
            }

            return false;
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