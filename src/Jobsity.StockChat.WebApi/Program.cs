using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Jobsity.StockChat.WebApi
{
    public class Program
    {
        private Program() { }

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
