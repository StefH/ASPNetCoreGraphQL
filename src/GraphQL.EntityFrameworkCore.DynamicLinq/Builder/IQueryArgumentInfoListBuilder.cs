using System;
using GraphQL.EntityFrameworkCore.DynamicLinq.Models;
using JetBrains.Annotations;

namespace GraphQL.EntityFrameworkCore.DynamicLinq.Builder
{
    public interface IQueryArgumentInfoListBuilder
    {
        QueryArgumentInfoList Build<T>();

        QueryArgumentInfoList Build([NotNull] Type graphQLType);
    }
}