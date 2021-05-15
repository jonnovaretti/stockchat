using Jobsity.StockChat.Application.Settings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace Jobsity.StockChat.Tests.IntegratedTests.Fixture
{
    public class WebApplicationFixture<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        public IAuthSetting AuthSetting { get; private set; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices((buider, services) =>
            {
                AuthSetting = buider.Configuration.GetSection("Authentication").Get<AuthSetting>();
            });
        }
    }
}
