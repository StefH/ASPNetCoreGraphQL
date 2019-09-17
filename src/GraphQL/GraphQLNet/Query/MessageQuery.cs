using System.Collections.Generic;
using System.Linq;
using GraphQL.Authorization;
using GraphQL.Types;

namespace GraphQLNet.Query
{
    public class MessageQuery : ObjectGraphType<Message>
    {
        public MessageQuery()
        {
            //Field(o => o.Content).Resolve(o => "This is Content").AuthorizeWith("Authorized");

            var messages = new List<Message> {new Message {Id = 1, Content = "C1"}, new Message { Id = 2, Content = "C2" } };

            Field<ListGraphType<MessageType>>("messages",
                arguments: new QueryArguments(new List<QueryArgument>
                {
                    new QueryArgument<IdGraphType>
                    {
                        Name = "id"
                    }
                }),
                resolve: context =>
                {
                    var id = context.GetArgument<int?>("id");
                    if (id.HasValue)
                    {
                        return messages.Where(m => m.Id == id);
                    }

                    return messages;
                }
            ).AuthorizeWith("Authorized");
        }
    }
}
