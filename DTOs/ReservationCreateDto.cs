namespace RestaurantSystem.Web.DTOs;

public class ReservationCreateDto
{
    public DateTime ReservationDateTime { get; set; }
    public int Persons { get; set; }
    public int ClientId { get; set; }
    public int RestaurantTableId { get; set; }
}
