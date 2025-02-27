using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Proteinx.Web.Controllers
{
    public class AuthController : Controller
    {

        private readonly HttpClient _httpClient;

        public AuthController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7202/");
        }


        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Index()   
        {
            return View();
        }


        [HttpPost]
public async Task<IActionResult> Login(LoginRequest login)
{
    var res = await _httpClient.PostAsJsonAsync("auth/login", login);
    if (res.IsSuccessStatusCode)
    {
        var result = await res.Content.ReadFromJsonAsync<TokenResponse>();
        if (result?.token != null)
        {
            HttpContext.Session.SetString("JWTToken", result.token);  //  Store token in session
                    Debug.WriteLine("Token Attached: " + result.token);
                    return RedirectToAction("Index", "Coupon");               //  Redirect to Coupon page
        }
    }
    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
    return View();
}

        public class TokenResponse
        {
            public string? token { get; set; }
        }

    }
}
