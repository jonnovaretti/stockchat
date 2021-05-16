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

namespace Jobsity.StockChat.WebApi.Extensions
{
    public static class ServicesExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddScoped<ITokenService, TokenService>();
        }

        public static void AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var authSettings = configuration.GetSection("Authentication").Get<AuthSetting>();
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
                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hub"))
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
