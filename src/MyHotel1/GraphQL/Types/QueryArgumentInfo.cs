using GraphQL.Types;

namespace MyHotel.GraphQL.Types
{
    public class QueryArgumentInfo
    {
        public QueryArgument QueryArgument { get; set; }

        public string GraphPropertyPath { get; set; }

        public string EntityPropertyPath { get; set; }
    }
}