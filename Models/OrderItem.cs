using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantSystem.Web.Models;

public class OrderItem
{
    public int OrderItemId { get; set; }

    public int OrderId { get; set; }
    public Order? Order { get; set; }

    public int MenuItemId { get; set; }
    public MenuItem? MenuItem { get; set; }

    [Range(1, 100)]
    public int Quantity { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    [Range(0.01, 100000)]
    public decimal UnitPrice { get; set; }
}
