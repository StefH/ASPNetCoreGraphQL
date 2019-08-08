using GraphQL.Types;

namespace GraphQLNet.Query
{
    public class MessageType : ObjectGraphType<Message>
    {
        public MessageType()
        {
            Field(x => x.Id);
            Field(x => x.Content);
        }
    }
}
