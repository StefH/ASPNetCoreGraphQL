using MyHotel.GraphQL.Types;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace MyHotel.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable ApplyQueryArguments(this IQueryable query, ICollection<QueryArgumentInfo> list, Dictionary<string, object> arguments)
        {
            if (arguments != null)
            {
                foreach (var argument in arguments)
                {
                    var info = list.FirstOrDefault(i => i.QueryArgument.Name == argument.Key);
                    if (info != null)
                    {
                        query = query.Where($"np({info.EntityPropertyPath}) == {argument.Value}");
                    }
                }
            }

            return query;
        }
    }
}