using Bogus;
using FluentAssertions;
using Jobsity.StockChat.WebApi.Services;
using Jobsity.StockChat.WebApi.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Xunit;

namespace Jobsity.StockChat.Tests.IntegratedTests.Services
{
    [Trait("Integrated tests", "Services")]
    public class TokenServiceTests
    {
        private readonly Faker _faker = new Faker();
        private ITokenService _tokenService;

        [Fact(DisplayName = "Given name and role when generate token, then token has to have name and role in claims")]
        public void Given_Name_Role_When_Generate_Token_Then_Token_Has_To_Have_Name_And_Role_Claims()
        {
            //Arrange
            var name = _faker.Name.FirstName();
            var role = _faker.Random.Words();

            _tokenService = new TokenService(new AuthSetting() { Secret = "fdghjaf7d8863b48e197b9287d492b708e3443fsdf" });

            //Act
            var token = _tokenService.GenerateToken(name, role);

            //Assert
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenOpened = tokenHandler.ReadJwtToken(token);

            tokenOpened.Claims.Any(c => c.Value == name).Should().BeTrue();
            tokenOpened.Claims.Any(c => c.Value == role).Should().BeTrue();
        }
    }
}