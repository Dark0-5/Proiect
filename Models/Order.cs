using System.ComponentModel.DataAnnotations;

namespace RestaurantSystem.Web.Models;

public class Order
{
    public int OrderId { get; set; }

    [Required]
    public DateTime OrderDateTime { get; set; } = DateTime.Now;

    [Required, StringLength(20)]
    public string Status { get; set; } = "New"; // New/InKitchen/Served/Cancelled

    public int ReservationId { get; set; }
    public Reservation? Reservation { get; set; }

    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
}
