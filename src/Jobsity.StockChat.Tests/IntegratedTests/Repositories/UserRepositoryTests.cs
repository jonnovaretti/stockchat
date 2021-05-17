using FluentAssertions;
using Jobsity.StockChat.Application.Models;
using Jobsity.StockChat.Application.Infrastructure.Repositories;
using Xunit;

namespace Jobsity.StockChat.Tests.IntegratedTests.Repositories
{
    [Trait("Integrated tests", "Repositories")]
    public class UserRepositoryTests
    {
        private readonly IUserRepository _userRepository;

        public UserRepositoryTests()
        {
            _userRepository = new UserRepository();
        }

        [Fact(DisplayName = "Given an invalid credentials when query user with username and passawork then return null")]
        public void Given_Invalid_Credentials_When_Get_User_Then_Return_Null()
        {
            //Arrange
            var user = new User
            {
                Username = "AnyUser",
                Password = "21124"
            };

            //Act
            var userFound = _userRepository.Get(user.Username, user.Password);

            //Assert
            userFound.Should().BeNull();
        }

        [Fact(DisplayName = "Given a valid credentials when query user with username and password then return the user")]
        public void Given_Valid_Credentials_When_Get_User_Then_Return_Found_User()
        {
            //Arrange
            var user = new User
            {
                Username = "Jhon",
                Password = "12345jhon"
            };

            //Act
            var userFound = _userRepository.Get(user.Username, user.Password);

            //Assert
            userFound.Should().NotBeNull();
            userFound.Username.Should().Be(user.Username);
            userFound.Password.Should().Be(user.Password);
        }
    }
}