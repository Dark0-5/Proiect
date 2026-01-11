using System.ComponentModel.DataAnnotations;

namespace RestaurantSystem.Web.Models;

public class Client
{
    public int ClientId { get; set; }

    [Required, StringLength(80)]
    public string FullName { get; set; } = string.Empty;

    [Required, StringLength(20)]
    public string Phone { get; set; } = string.Empty;

    public int UserId { get; set; }
    public User? User { get; set; }

    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
