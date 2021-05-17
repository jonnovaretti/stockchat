using Jobsity.StockChat.Application.Constants;
using Jobsity.StockChat.Application.Infrastructure.MessageBroker;
using Jobsity.StockChat.Application.Services;
using Jobsity.StockChat.Application.Settings;
using Jobsity.StockChat.Workers.Consumers;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Jobsity.StockChat.Workers.Extensions
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IStockRequestService, StockRequestService>();
            services.AddSingleton<IStockQuotePublisher, StockQuotePublisher>();
            services.AddSingleton<Application.Infrastructure.MessageBroker.IBusFactory, BusFactory>();
            services.AddSingleton<IPublisher, Publisher>();
            
            services.AddHttpClient();

            return services;
        }

        public static IServiceCollection AddSettings(this IServiceCollection services, IConfiguration configuration)
        {
            IStooqSetting stooqSetting = new StooqSetting();
            configuration.GetSection(SettingSections.SqootSetting).Bind(stooqSetting);

            IMessageBrokerSetting messageBrokerSetting = new MessageBrokerSetting();
            configuration.GetSection(SettingSections.MessageBrokerSetting).Bind(messageBrokerSetting);

            services.AddSingleton(messageBrokerSetting);
            services.AddSingleton(stooqSetting);

            return services;
        }

        public static IServiceCollection AddMassTransit(this IServiceCollection services, IConfiguration configuration)
        {
            var serviceProvider = services.BuildServiceProvider();

            var messageBrokerSetting = new MessageBrokerSetting();
            configuration.GetSection(SettingSections.MessageBrokerSetting).Bind(messageBrokerSetting);

            services.AddMassTransit(bus =>
            {
                bus.UsingRabbitMq((ctx, busConfigurator) =>
                {
                    busConfigurator.Host(messageBrokerSetting.Host, messageBrokerSetting.Vhost, acc =>
                    {
                        acc.Username(messageBrokerSetting.Username);
                        acc.Password(messageBrokerSetting.Password);
                    });

                    busConfigurator.ReceiveEndpoint(QueueNames.RequestStockQuote, x =>
                    {
                        x.Consumer(() =>
                        {
                            return new StockCommandConsumer(serviceProvider.GetService<IStockRequestService>(), serviceProvider.GetService<IStockQuotePublisher>());
                        });
                    });
                });
            });

            return services;
        }
    }
}
