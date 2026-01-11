namespace RestaurantSystem.Web.DTOs;

public class ReservationListDto
{
    public int ReservationId { get; set; }
    public DateTime ReservationDateTime { get; set; }
    public string Status { get; set; } = string.Empty;
    public int Persons { get; set; }
    public string Table { get; set; } = string.Empty;
}
