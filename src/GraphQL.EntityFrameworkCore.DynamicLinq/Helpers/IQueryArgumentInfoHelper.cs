using System;
using System.Collections.Generic;
using GraphQL.EntityFrameworkCore.DynamicLinq.Models;
using JetBrains.Annotations;

namespace GraphQL.EntityFrameworkCore.DynamicLinq.Helpers
{
    public interface IQueryArgumentInfoHelper
    {
        ICollection<QueryArgumentInfo> PopulateQueryArgumentInfoList<T>();

        ICollection<QueryArgumentInfo> PopulateQueryArgumentInfoList([NotNull] Type graphQLType);
    }
}