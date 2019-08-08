using GraphQL.Types;

namespace GraphQL.EntityFrameworkCore.DynamicLinq.Tests.Utils.Types
{
    public class QueryArgumentInfo
    {
        public QueryArgument QueryArgument { get; set; }

        public string GraphPropertyPath { get; set; }

        public string EntityPropertyPath { get; set; }
    }
}