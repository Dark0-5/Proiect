using System.ComponentModel.DataAnnotations;

namespace RestaurantSystem.Web.ViewModels;

public class RegisterVm
{
    [Required, StringLength(80)]
    public string FullName { get; set; } = string.Empty;

    [Required, StringLength(20)]
    public string Phone { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, MinLength(6), DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Required, Compare(nameof(Password)), DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = string.Empty;
}
