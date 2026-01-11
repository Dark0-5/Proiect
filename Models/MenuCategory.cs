using System.ComponentModel.DataAnnotations;

namespace RestaurantSystem.Web.Models;

public class MenuCategory
{
    public int MenuCategoryId { get; set; }

    [Required, StringLength(50)]
    public string Name { get; set; } = string.Empty;

    public ICollection<MenuItem> Items { get; set; } = new List<MenuItem>();
}
