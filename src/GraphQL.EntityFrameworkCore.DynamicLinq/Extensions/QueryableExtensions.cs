﻿using System.Linq;
using GraphQL.EntityFrameworkCore.DynamicLinq.Builder;
using GraphQL.EntityFrameworkCore.DynamicLinq.Models;
using GraphQL.EntityFrameworkCore.DynamicLinq.Validation;
using GraphQL.Types;
using JetBrains.Annotations;

namespace GraphQL.EntityFrameworkCore.DynamicLinq.Extensions
{
    public static class QueryableExtensions
    {
        [PublicAPI]
        public static IQueryable<T> ApplyQueryArguments<T>([NotNull] this IQueryable<T> query, [NotNull] QueryArgumentInfoList list, [NotNull] ResolveFieldContext<object> context)
        {
            Guard.NotNull(query, nameof(query));
            Guard.HasNoNulls(list, nameof(list));
            Guard.NotNull(context, nameof(context));

            return new DynamicQueryableBuilder<T, object>(query, list, context).Build();
        }
    }
}