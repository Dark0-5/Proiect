using System.ComponentModel.DataAnnotations;

namespace RestaurantSystem.Web.Models;

public class Reservation
{
    public int ReservationId { get; set; }

    [Required]
    public DateTime ReservationDateTime { get; set; }

    [Range(1, 20)]
    public int Persons { get; set; }

    [Required, StringLength(20)]
    public string Status { get; set; } = "Pending"; // Pending/Confirmed/Cancelled/Completed

    public int ClientId { get; set; }
    public Client? Client { get; set; }

    public int RestaurantTableId { get; set; }
    public RestaurantTable? RestaurantTable { get; set; }

    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
