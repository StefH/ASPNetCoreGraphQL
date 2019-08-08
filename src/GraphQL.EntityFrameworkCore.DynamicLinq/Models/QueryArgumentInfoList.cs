using System;
using System.Collections.Generic;
using System.Linq;
using GraphQL.EntityFrameworkCore.DynamicLinq.Validation;
using JetBrains.Annotations;

namespace GraphQL.EntityFrameworkCore.DynamicLinq.Models
{
    public class QueryArgumentInfoList : List<QueryArgumentInfo>
    {
        public QueryArgumentInfoList()
        {
        }

        private QueryArgumentInfoList(IEnumerable<QueryArgumentInfo> collection)
        {
            AddRange(collection);
        }

        public QueryArgumentInfoList Include([NotNull] params string[] includedGraphQLPropertyPaths)
        {
            Guard.HasNoNulls(includedGraphQLPropertyPaths, nameof(includedGraphQLPropertyPaths));

            return new QueryArgumentInfoList(this.Where(q => includedGraphQLPropertyPaths.Contains(q.GraphQLPath)));
        }

        public QueryArgumentInfoList Exclude([NotNull] params string[] excludedGraphQLPropertyPaths)
        {
            Guard.HasNoNulls(excludedGraphQLPropertyPaths, nameof(excludedGraphQLPropertyPaths));

            return new QueryArgumentInfoList(this.Where(q => !excludedGraphQLPropertyPaths.Contains(q.GraphQLPath)));
        }

        public QueryArgumentInfoList Filter([NotNull] Predicate<string> predicate)
        {
            Guard.NotNull(predicate, nameof(predicate));

            return new QueryArgumentInfoList(this.Where(q => predicate(q.GraphQLPath)));
        }
    }
}