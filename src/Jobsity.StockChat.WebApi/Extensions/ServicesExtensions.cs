using Jobsity.StockChat.Application.Constants;
using Jobsity.StockChat.Application.Infrastructure.MessageBroker;
using Jobsity.StockChat.Application.Infrastructure.Repositories;
using Jobsity.StockChat.Application.Services;
using Jobsity.StockChat.Application.Settings;
using Jobsity.StockChat.WebApi.Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using Jobsity.StockChat.WebApi.Consumers;

namespace Jobsity.StockChat.WebApi.Extensions
{
    public static class ServicesExtensions
    {
        private static readonly ResponseStockQuoteConsumer consumer = new ResponseStockQuoteConsumer();

        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            var messageBrokerSetting = new MessageBrokerSetting();
            configuration.GetSection(SettingSections.MessageBrokerSetting).Bind(messageBrokerSetting);

            services.AddSingleton<IMessageBrokerSetting>(messageBrokerSetting);
            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddSingleton<IMessageAnalyserService, MessageAnalyserService>();
            services.AddSingleton<ICommandPublisher, CommandPublisher>();
            services.AddSingleton<Application.Infrastructure.MessageBroker.IBusFactory, BusFactory>();
            services.AddSingleton<IPublisher, Publisher>();
            services.AddSingleton<IConsumerObservable>(consumer);

            return services;
        }

        public static IServiceCollection AddMessageBroker(this IServiceCollection services, IConfiguration configuration)
        {
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

                    busConfigurator.ReceiveEndpoint(QueueNames.ResponseStockQuote, x =>
                    {
                        x.Consumer(() =>
                        {
                            return consumer;
                        });
                    });
                });
            });

            services.AddMassTransitHostedService();

            return services;
        }

        public static void AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var authSettings = configuration.GetSection(SettingSections.Authentication).Get<AuthSetting>();
            var key = Encoding.ASCII.GetBytes(authSettings.Secret);

            services.AddSingleton<IAuthSetting>(authSettings);
            services.AddSingleton<IUserIdProvider, UserIdProvider>();

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/chat"))
                        {
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    }
                };
            });
        }
    }
}
