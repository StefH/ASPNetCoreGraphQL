using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
 

namespace NHLStats.Api
{
    public class Program
    {
        private const string url = "http://localhost:51111";

        public static void Main(string[] args)
        {
            Console.WriteLine($"API runs @ {url}/graphql");
            Console.WriteLine($"GraphiQL runs @ {url}/ui/graphiql");
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls(url)
                .Build();
    }
}
