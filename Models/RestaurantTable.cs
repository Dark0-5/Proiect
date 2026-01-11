using System.ComponentModel.DataAnnotations;

namespace RestaurantSystem.Web.Models;

public class RestaurantTable
{
    public int RestaurantTableId { get; set; }

    [Required]
    public int TableNumber { get; set; }

    [Range(1, 20)]
    public int Seats { get; set; }

    [Required, StringLength(30)]
    public string Zone { get; set; } = "Interior";

    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
