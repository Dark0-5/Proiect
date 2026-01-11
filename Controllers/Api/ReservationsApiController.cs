using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantSystem.Web.Data;
using RestaurantSystem.Web.DTOs;

namespace RestaurantSystem.Web.Controllers.Api;

[ApiController]
[Route("api/reservations")]
public class ReservationsApiController : ControllerBase
{
    private readonly AppDbContext _context;

    public ReservationsApiController(AppDbContext context)
    {
        _context = context;
    }

    // GET api/reservations/client/5
    [HttpGet("client/{clientId}")]
    public async Task<ActionResult<IEnumerable<ReservationListDto>>> GetForClient(int clientId)
    {
        var reservations = await _context.Reservations
            .Where(r => r.ClientId == clientId)
            .Include(r => r.RestaurantTable)
            .Select(r => new ReservationListDto
            {
                ReservationId = r.ReservationId,
                ReservationDateTime = r.ReservationDateTime,
                Persons = r.Persons,
                Status = r.Status,
                Table = "Masa " + r.RestaurantTable.TableNumber
            })
            .ToListAsync();

        return reservations;
    }

    // POST api/reservations
    [HttpPost]
    public async Task<IActionResult> Create(ReservationCreateDto dto)
    {
        var reservation = new Models.Reservation
        {
            ReservationDateTime = dto.ReservationDateTime,
            Persons = dto.Persons,
            ClientId = dto.ClientId,
            RestaurantTableId = dto.RestaurantTableId,
            Status = "Pending"
        };

        _context.Reservations.Add(reservation);
        await _context.SaveChangesAsync();

        return Ok();
    }
}
