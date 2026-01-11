using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantSystem.Web.Models;

public class MenuItem
{
    public int MenuItemId { get; set; }

    [Required, StringLength(80)]
    public string Name { get; set; } = string.Empty;

    [Column(TypeName = "decimal(10,2)")]
    [Range(0.01, 100000)]
    public decimal Price { get; set; }

    public bool IsAvailable { get; set; } = true;

    public int MenuCategoryId { get; set; }
    public MenuCategory? MenuCategory { get; set; }

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
