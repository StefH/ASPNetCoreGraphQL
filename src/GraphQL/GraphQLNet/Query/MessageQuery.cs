using GraphQL.Authorization;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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

        public byte[] ConvertToBytes(string fileName)
        {
            using (Stream stream = File.Open(fileName, FileMode.Open))
            {
                var bytes = new byte[stream.Length];
                stream.Read(bytes, 0, (int)stream.Length);
                return bytes;
            }
        }
    }
}
