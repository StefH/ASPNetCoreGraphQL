using System;
using System.Collections.Generic;
using System.Linq;
using GraphQL.EntityFrameworkCore.DynamicLinq.Extensions;
using GraphQL.EntityFrameworkCore.DynamicLinq.Models;
using GraphQL.EntityFrameworkCore.DynamicLinq.Resolvers;
using GraphQL.EntityFrameworkCore.DynamicLinq.Validation;
using GraphQL.Types;

namespace GraphQL.EntityFrameworkCore.DynamicLinq.Helpers
{
    internal class QueryArgumentInfoHelper : IQueryArgumentInfoHelper
    {
        private readonly IPropertyPathResolver _propertyPathResolver;

        public QueryArgumentInfoHelper(IPropertyPathResolver propertyPathResolver)
        {
            Guard.NotNull(propertyPathResolver, nameof(propertyPathResolver));

            _propertyPathResolver = propertyPathResolver;
        }

        public ICollection<QueryArgumentInfo> PopulateQueryArgumentInfoList<T>()
        {
            return PopulateQueryArgumentInfoList(typeof(T));
        }

        public ICollection<QueryArgumentInfo> PopulateQueryArgumentInfoList(Type graphQLType)
        {
            Guard.NotNull(graphQLType, nameof(graphQLType));

            return PopulateQueryArgumentInfoList(graphQLType, string.Empty, string.Empty);
        }

        private ICollection<QueryArgumentInfo> PopulateQueryArgumentInfoList(Type graphQLType, string parentGraphQLPath, string parentEntityPath)
        {
            var list = new List<QueryArgumentInfo>();
            if (!(Activator.CreateInstance(graphQLType) is IComplexGraphType complexGraphQLInstance))
            {
                return list;
            }

            complexGraphQLInstance.Fields.ToList().ForEach(ft =>
            {
                string graphPath = $"{parentGraphQLPath}{ft.Name}";

                Type thisModel = graphQLType.ModelType();
                string resolvedParentEntityPath = _propertyPathResolver.Resolve(thisModel, ft.Name);
                string entityPath = !string.IsNullOrEmpty(parentEntityPath) ? $"{parentEntityPath}.{resolvedParentEntityPath}" : resolvedParentEntityPath;

                Type childGraphQLType = ft.Type.GraphType();
                if (childGraphQLType.IsObjectGraphType())
                {
                    list.AddRange(PopulateQueryArgumentInfoList(childGraphQLType, graphPath, entityPath));
                }
                else
                {
                    list.Add(new QueryArgumentInfo
                    {
                        QueryArgument = new QueryArgument(childGraphQLType) { Name = graphPath },
                        GraphPath = graphPath,
                        EntityPath = entityPath
                    });
                }
            });

            return list;
        }
    }
}