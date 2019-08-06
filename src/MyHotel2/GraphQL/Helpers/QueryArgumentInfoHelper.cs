using GraphQL.Types;
using MyHotel.AutoMapper;
using MyHotel.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyHotel.GraphQL.Helpers
{
    public class QueryArgumentInfoHelper : IQueryArgumentInfoHelper
    {
        private readonly IAutoMapperPropertyNameResolver _autoMapperPropertyNameResolver;

        public QueryArgumentInfoHelper(IAutoMapperPropertyNameResolver autoMapperPropertyNameResolver)
        {
            _autoMapperPropertyNameResolver = autoMapperPropertyNameResolver;
        }

        public ICollection<QueryArgumentInfo> PopulateQueryArgumentInfoList<T>()
        {
            return PopulateQueryArgumentInfoList(typeof(T), null, "", "");
        }

        private ICollection<QueryArgumentInfo> PopulateQueryArgumentInfoList(Type type, Type parentType, string graphPathPrefix, string entityPathPrefix)
        {
            var list = new List<QueryArgumentInfo>();
            if (!(Activator.CreateInstance(type) is IComplexGraphType complexGraphInstance))
            {
                return list;
            }

            complexGraphInstance.Fields.ToList().ForEach(fieldType =>
            {
                Type graphType = fieldType.Type.GraphType();
                Type parentModel = parentType?.ModelType();

                string resolvedParentEntityPath = _autoMapperPropertyNameResolver.Resolve(parentModel, graphPathPrefix);

                string graphPath = $"{graphPathPrefix}{fieldType.Name}";
                string entityPath = !string.IsNullOrEmpty(entityPathPrefix) ? $"{entityPathPrefix}.{resolvedParentEntityPath}" : resolvedParentEntityPath;

                if (graphType.IsObjectGraphType())
                {
                    list.AddRange(PopulateQueryArgumentInfoList(graphType, type, graphPath, entityPath));
                }
                else
                {
                    Type thisModel = type.ModelType();
                    string resolvedPropertyName = _autoMapperPropertyNameResolver.Resolve(thisModel, fieldType.Name);

                    list.Add(new QueryArgumentInfo
                    {
                        QueryArgument = new QueryArgument(graphType) { Name = graphPath },
                        GraphPath = graphPath,
                        EntityPath = !string.IsNullOrEmpty(entityPath) ? $"{entityPath}.{resolvedPropertyName}" : resolvedPropertyName
                    });
                }
            });

            return list;
        }
    }
}
