using GraphQL.Types;

namespace MyHotel.GraphQL.Types
{
    public class GuestInputType : InputObjectGraphType<GuestType>
    {
        public GuestInputType()
        {
            Name = "GuestInput";
            Field<NonNullGraphType<StringGraphType>>("name");
            Field<DateGraphType>("registerDate");
        }
    }
}