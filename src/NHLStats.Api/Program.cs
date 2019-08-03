using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
 

namespace NHLStats.Api
{
    public class Program
    {
        private const string url = "http://localhost:5000/";

        public static void Main(string[] args)
        {
            Console.WriteLine($"Access GraphiQL in-browser tool @ {url}graphql");
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls(url)
                .Build();
    }
}
