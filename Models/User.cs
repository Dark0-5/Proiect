using System.ComponentModel.DataAnnotations;

namespace RestaurantSystem.Web.Models;

public class User
{
    public int UserId { get; set; }

    [Required, EmailAddress, StringLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(200)]
    public string PasswordHash { get; set; } = string.Empty;

    [Required, StringLength(20)]
    public string Role { get; set; } = "Client"; // Admin / Staff / Client

    public Client? Client { get; set; }
}
