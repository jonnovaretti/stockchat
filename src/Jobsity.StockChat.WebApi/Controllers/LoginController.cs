using Jobsity.StockChat.Application.Infrastructure.Repositories;
using Jobsity.StockChat.Application.Services;
using Jobsity.StockChat.WebApi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace Jobsity.StockChat.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class LoginController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly ILogger<LoginController> _logger;

        public LoginController(IUserRepository userRepository, ITokenService tokenService, ILogger<LoginController> logger)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _logger = logger;
        }

        [HttpPost]
        [Route("login")]
        public ActionResult Authenticate([FromBody] UserRequest request)
        {
            try
            {
                var userFound = _userRepository.Get(request.Username, request.Password);

                if (userFound is null)
                    return Unauthorized(new Response("Invalid credentials"));

                var token = _tokenService.GenerateToken(userFound.Username, userFound.Role);

                return Ok(new Response<AuthenticateResponse>(new AuthenticateResponse(userFound.Username, token)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Internal error on {LoginController}", nameof(LoginController));
                return BadRequest();
            }
        }
    }
}
