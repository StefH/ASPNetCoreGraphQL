using System;
using JetBrains.Annotations;

namespace GraphQL.EntityFrameworkCore.DynamicLinq.Resolvers
{
    public interface IPropertyPathResolver
    {
        string Resolve([NotNull] Type sourceType, [NotNull] string sourcepropertyPath, [CanBeNull] Type destinationType = null);
    }
}