using GraphQL.Types;

namespace GraphQL.EntityFrameworkCore.DynamicLinq.Models
{
    public class QueryArgumentInfo
    {
        public QueryArgument QueryArgument { get; set; }

        public string GraphPath { get; set; }

        public string EntityPath { get; set; }
    }
}