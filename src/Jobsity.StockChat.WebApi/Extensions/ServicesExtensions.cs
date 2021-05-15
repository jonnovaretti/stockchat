﻿using Jobsity.StockChat.WebApi.Repositories;
using Jobsity.StockChat.WebApi.Services;
using Jobsity.StockChat.WebApi.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }
    }
}