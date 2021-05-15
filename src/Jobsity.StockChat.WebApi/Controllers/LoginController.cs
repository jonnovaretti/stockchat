using Jobsity.StockChat.WebApi.Repositories;
using Jobsity.StockChat.WebApi.Services;
using Jobsity.StockChat.WebApi.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Jobsity.StockChat.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class LoginController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public LoginController(IUserRepository userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        [HttpPost]
        [Route("login")]
        public ActionResult Authenticate([FromBody] UserRequest request)
        {
            var userFound = _userRepository.Get(request.Username, request.Password);

            if (userFound is null)
                return NotFound(new Response("Invalid credentials"));

            var token = _tokenService.GenerateToken(userFound);

            return Ok(new Response<AuthenticateResponse>(new AuthenticateResponse(userFound.Username, token)));
        }
    }
}
