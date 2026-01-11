using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantSystem.Web.Data;
using RestaurantSystem.Web.DTOs;
using RestaurantSystem.Web.Security;

namespace RestaurantSystem.Web.Controllers.Api;

[ApiController]
[Route("api/auth")]
public class AuthApiController : ControllerBase
{
    private readonly AppDbContext _context;

    public AuthApiController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResultDto>> Login(LoginDto dto)
    {
        var email = dto.Email.Trim().ToLower();
        var hash = PasswordHasher.Hash(dto.Password);

        var user = await _context.Users
            .FirstOrDefaultAsync(u =>
                u.Email.ToLower() == email &&
                u.PasswordHash == hash &&
                u.Role == "Client");

        if (user == null)
        {
            return new LoginResultDto
            {
                Success = false,
                Message = "Email sau parola incorecte."
            };
        }

        var client = await _context.Clients
            .FirstAsync(c => c.UserId == user.UserId);

        return new LoginResultDto
        {
            Success = true,
            ClientId = client.ClientId,
            Message = "Login reusit"
        };
    }
}
