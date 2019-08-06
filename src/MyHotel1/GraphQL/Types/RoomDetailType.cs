using GraphQL.Types;
using MyHotel.Entities;

namespace MyHotel.GraphQL.Types
{
    public class RoomDetailType : ObjectGraphType<RoomDetail>
    {
        public RoomDetailType()
        {
            Field(x => x.Id);
            Field(x => x.Windows);
            Field(x => x.Beds);
        }
    }
}