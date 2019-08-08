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

            return PopulateQueryArgumentInfoList(graphQLType, null, "", "");
        }

        private ICollection<QueryArgumentInfo> PopulateQueryArgumentInfoList(Type graphQLType, Type parentGraphQLType, string parentGraphQLPath, string parentEntityPath)
        {
            var list = new List<QueryArgumentInfo>();
            if (!(Activator.CreateInstance(graphQLType) is IComplexGraphType complexGraphInstance))
            {
                return list;
            }

            complexGraphInstance.Fields.ToList().ForEach(ft =>
            {
                string graphPath = $"{parentGraphQLPath}{ft.Name}";
                // Type underlyingParentType = parentGraphQLType?.ModelType();
                Type thisModel = graphQLType.ModelType();

                string resolvedParentEntityPath = _propertyPathResolver.Resolve(thisModel, ft.Name);
                string entityPath = !string.IsNullOrEmpty(parentEntityPath) ? $"{parentEntityPath}.{resolvedParentEntityPath}" : resolvedParentEntityPath;

                Type graphType = ft.Type.GraphType();
                if (graphType.IsObjectGraphType())
                {
                    list.AddRange(PopulateQueryArgumentInfoList(graphType, graphQLType, graphPath, entityPath));
                }
                else
                {
                    //Type thisModel = type.ModelType();
                    //string resolvedPropertyName = _propertyPathResolver.Resolve(thisModel, ft.Name);

                    list.Add(new QueryArgumentInfo
                    {
                        QueryArgument = new QueryArgument(graphType) { Name = graphPath },
                        GraphPath = graphPath,
                        EntityPath = entityPath
                    });
                }
            });

            return list;
        }
    }
}
