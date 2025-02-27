using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace Proteinx.Services.CouponApi.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : Controller
    {
        private readonly JwtTokenGenerator _jwtTokenGenerator;

        public AuthController(JwtTokenGenerator jwtTokenGenerator)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
        }
        [HttpGet("index")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody]  LoginRequest login)
        {

            if (login.Email == "admin" && login.Password == "password")
            {
                var token = _jwtTokenGenerator.GenerateToken(login.Email);
                return Ok(new { token });
            }
            return Unauthorized("Invalid credentials.");

        }

        public class LoginRequest
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

    }
}
