using GraphQL.Types;

namespace GraphQL.EntityFrameworkCore.DynamicLinq.Models
{
    public class QueryArgumentInfo
    {
        public QueryArgument QueryArgument { get; internal set; }

        public string GraphQLPath { get; internal set; }

        public string EntityPath { get; internal set; }
    }
}