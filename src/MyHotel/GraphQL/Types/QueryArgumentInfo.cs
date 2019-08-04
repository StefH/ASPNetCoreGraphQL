using System;
using GraphQL.Types;

namespace MyHotel.GraphQL.Types
{
    public class QueryArgumentInfo
    {
        public QueryArgument QueryArgument { get; set; }

        //public Type ParentModel { get; set; }

        //public Type Model { get; set; }

        //public string ModelPropertyName { get; set; }

        public string EntityPropertyPath { get; set; }
    }
}