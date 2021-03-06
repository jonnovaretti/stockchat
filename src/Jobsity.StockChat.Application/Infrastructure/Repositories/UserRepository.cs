using Jobsity.StockChat.Application.Models;
using System.Collections.Generic;
using System.Linq;

namespace Jobsity.StockChat.Application.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        public User Get(string username, string password)
        {
            var users = new List<User>
            {
                new User { Id = 1, Username = "Jhon", Password = "12345jhon", Role = "user" },
                new User { Id = 2, Username = "Luiz", Password = "12345luiz", Role = "user" }
            };

            return users.FirstOrDefault(x => x.Username.ToLower() == username.ToLower() && x.Password == password);
        }
    }
}
