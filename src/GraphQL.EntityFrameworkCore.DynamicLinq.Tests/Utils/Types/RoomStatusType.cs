using GraphQL.EntityFrameworkCore.DynamicLinq.Tests.Utils.Entities;
using GraphQL.Types;

namespace GraphQL.EntityFrameworkCore.DynamicLinq.Tests.Utils.Types
{
    public class RoomStatusType : EnumerationGraphType<RoomStatus>
    {
        public RoomStatusType()
        {
            Description = "Shows if the room is available or not.";
        }
    }
}
