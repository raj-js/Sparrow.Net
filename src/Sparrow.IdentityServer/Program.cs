using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Sparrow.IdentityServer.SeedWork;
using System.Threading.Tasks;

namespace Sparrow.IdentityServer
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            await host.Services.AddSeedDataAsync();

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
