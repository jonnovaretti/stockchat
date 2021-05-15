using Jobsity.StockChat.WebApi.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Jobsity.StockChat.Tests.IntegratedTests.Fixture
{
    public class WebApplicationFixture<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        private readonly IConfiguration _configuration;

        public WebApplicationFixture(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddAuthentication(_configuration);
            });
        }
    }
}
