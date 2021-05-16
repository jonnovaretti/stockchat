using Jobsity.StockChat.Workers.Consumers;
using Jobsity.StockChat.Workers.Extensions;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Jobsity.StockChat.Workers
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services.AddServices();
                    services.AddSettings(context.Configuration);
                    services.AddMassTransit(context.Configuration);
                    
                    services.AddMassTransitHostedService();
                    services.AddHostedService<ConsumerBase>();
                });
    }
}
