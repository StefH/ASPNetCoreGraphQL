using GraphQL.Types;

namespace MyHotel.GraphQL.Helpers
{
    public class QueryArgumentInfo
    {
        public QueryArgument QueryArgument { get; set; }

        public string GraphPath { get; set; }

        public string EntityPath { get; set; }
    }
}