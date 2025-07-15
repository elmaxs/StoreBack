using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Application.Abstractions.User;
using Store.Contracts.Users;

namespace Store.API.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly HttpContext _httpContext;
        private readonly IUserService _userService;

        public UserController(IUserService userService, HttpContext httpContext)
        {
            _userService = userService;
            _httpContext = httpContext;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            await _userService.Register(request.UserName, request.Email, request.Password);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserRequest request)
        {
            var token = await _userService.Login(request.Email, request.Password);

            _httpContext.Response.Cookies.Append("testy-cookies", token);

            return Ok();
        }
    }
}
