//using Microsoft.AspNetCore.Mvc;
//using PoliceBharatiLogin.DTOs;
//using PoliceBharatiLogin.Services;

//namespace PoliceBharatiLogin.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class LoginController : ControllerBase
//    {
//        private readonly LoginService _loginService;

//        public LoginController(LoginService loginService)
//        {
//            _loginService = loginService;
//        }

//        [HttpPost]
//        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
//        {
//            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
//                return BadRequest("Username and password are required.");

//            var user = await _loginService.AuthenticateAsync(request.Username, request.Password);

//            if (user == null)
//                return Unauthorized(new { message = "Invalid credentials" });

//            // Store user in HttpContext for access control
//            HttpContext.Items["User"] = user;

//            return Ok(user);
//        }
//    }
//}
using Microsoft.AspNetCore.Mvc;
using policebharati2026.DTOs;
using policebharati2026.Services;

namespace policebharati2026.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly LoginService _loginService;

        public LoginController(LoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest(new { message = "Username and password are required." });

            var user = await _loginService.AuthenticateAsync(request.Username, request.Password);

            if (user == null)
                return Unauthorized(new { message = "Invalid credentials" });

            return Ok(user);
        }
    }
}
