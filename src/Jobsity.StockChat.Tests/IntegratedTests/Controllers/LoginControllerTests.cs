using FluentAssertions;
using Jobsity.StockChat.Application.Services;
using Jobsity.StockChat.Tests.IntegratedTests.Fixture;
using Jobsity.StockChat.WebApi;
using Jobsity.StockChat.WebApi.ViewModels;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Jobsity.StockChat.Tests.IntegratedTests.Controllers
{
    [Trait("Integrated tests", "Controllers")]
    public class LoginControllerTests : IClassFixture<WebApplicationFixture<Startup>>
    {
        private readonly HttpClient _httpClient;
        private readonly WebApplicationFixture<Startup> _fixture;

        public LoginControllerTests(WebApplicationFixture<Startup> fixture)
        {
            _fixture = fixture;
            _httpClient = _fixture.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact(DisplayName ="Given a valid credentials when post to login, then return status code ok")]
        public async Task Given_Valid_Credentials_When_Post_Login_Then_Return_StatusCode_Ok()
        {
            //Arrange
            var userRequest = new UserRequest() { Username = "Jhon", Password = "12345jhon" };
            var payload = new StringContent(JsonSerializer.Serialize(userRequest), Encoding.UTF8, "application/json");

            //Act
            var response = await _httpClient.PostAsync("api/v1/login", payload);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact(DisplayName = "Given an invalid credentials when post to login, then return unauthorize")]
        public async Task Given_Invalid_Credentials_When_Post_Login_Then_Return_StatusCode_Unauthorize()
        {
            //Arrange
            var userRequest = new UserRequest() { Username = "Jhon", Password = "anypass" };
            var payload = new StringContent(JsonSerializer.Serialize(userRequest), Encoding.UTF8, "application/json");

            //Act
            var response = await _httpClient.PostAsync("api/v1/login", payload);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact(DisplayName = "Given a valid credentials when token service throw an exception, then return bad request")]
        public async Task Given_Valid_Credentials_When_Token_Service_Throw_An_Exception_Then_Return_Bad_Request()
        {
            //Arrange
            var userRequest = new UserRequest() { Username = "Jhon", Password = "12345jhon" };
            var tokenGenerator = Substitute.For<ITokenService>();

            var payload = new StringContent(JsonSerializer.Serialize(userRequest), Encoding.UTF8, "application/json");
            
            var httpClient = _fixture.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddScoped(s => { return tokenGenerator; });
                });
            }).CreateClient();

            tokenGenerator.When(s => s.GenerateToken(Arg.Any<string>(), Arg.Any<string>())).Throw(new System.Exception());

            //Act
            var response = await httpClient.PostAsync("api/v1/login", payload);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
