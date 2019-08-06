using System.Collections.Generic;

namespace MyHotel.GraphQL.Helpers
{
    public interface IQueryArgumentInfoHelper
    {
        ICollection<QueryArgumentInfo> PopulateQueryArgumentInfoList<T>();
    }
}