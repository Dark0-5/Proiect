
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantSystem.Web.Data;
using RestaurantSystem.Web.Models;
using RestaurantSystem.Web.Security;
using RestaurantSystem.Web.ViewModels;
using System.Security.Claims;

namespace RestaurantSystem.Web.Controllers;

public class AccountController : Controller
{
    private readonly AppDbContext _context;

    public AccountController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginVm vm)
    {
        if (!ModelState.IsValid) return View(vm);

        var email = vm.Email.Trim().ToLower();
        var pwdHash = PasswordHasher.Hash(vm.Password);

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email && u.PasswordHash == pwdHash);
        if (user == null)
        {
            ModelState.AddModelError("", "Email sau parola incorecte.");
            return View(vm);
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Register() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterVm vm)
    {
        if (!ModelState.IsValid) return View(vm);

        var email = vm.Email.Trim().ToLower();
        var exists = await _context.Users.AnyAsync(u => u.Email.ToLower() == email);
        if (exists)
        {
            ModelState.AddModelError(nameof(vm.Email), "Exista deja un cont cu acest email.");
            return View(vm);
        }

        var user = new User
        {
            Email = vm.Email.Trim(),
            PasswordHash = PasswordHasher.Hash(vm.Password),
            Role = "Client"
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var client = new Client
        {
            FullName = vm.FullName.Trim(),
            Phone = vm.Phone.Trim(),
            UserId = user.UserId
        };

        _context.Clients.Add(client);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Login));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction(nameof(Login));
    }

    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }
}
