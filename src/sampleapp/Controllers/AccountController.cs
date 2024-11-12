using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using sampleapp.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace sampleapp.Controllers;

[AllowAnonymous]
public class AccountController : Controller
{
    private readonly UserRepository _userRepository;
    private readonly ILogger<AccountController> _logger;

    public AccountController(IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection");
        _userRepository = new UserRepository(connectionString);
    }


    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {   
        if(ModelState.IsValid == true)
        {
            var user = _userRepository.ValidateUser(model.Username,model.Password);
            if(user == true)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.Username),
                    new Claim(ClaimTypes.Role,"admin")
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal  = new ClaimsPrincipal(claimsIdentity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, 
                claimsPrincipal, 
                new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.Now.AddHours(1)
                    });
                return RedirectToAction("index", "Home");
            }
             ModelState.AddModelError(string.Empty, "账号或密码错误。");
        }


        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        Console.Write("logout被执行了");
        return RedirectToAction("Login", "Account");
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
