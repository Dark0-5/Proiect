namespace RestaurantSystem.Web.DTOs;

public class LoginResultDto
{
    public bool Success { get; set; }
    public int? ClientId { get; set; }
    public string Message { get; set; } = string.Empty;
}
