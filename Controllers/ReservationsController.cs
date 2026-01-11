using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantSystem.Web.Data;
using RestaurantSystem.Web.Models;
using Microsoft.AspNetCore.Authorization;

namespace RestaurantSystem.Web.Controllers
{
    [Authorize(Roles = "Admin,Staff")]

    public class ReservationsController : Controller
    {
        private readonly AppDbContext _context;

        public ReservationsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Reservations
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Reservations
                .AsNoTracking()
                .Include(r => r.Client)
                .Include(r => r.RestaurantTable);

            return View(await appDbContext.ToListAsync());
        }

        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .AsNoTracking()
                .Include(r => r.Client)
                .Include(r => r.RestaurantTable)
                .FirstOrDefaultAsync(m => m.ReservationId == id);

            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: Reservations/Create
        public IActionResult Create()
        {
            ViewData["ClientId"] = new SelectList(_context.Clients, "ClientId", "FullName");

            // ✅ dropdown frumos pentru masa: "Masa 5 - Interior - 4 locuri"
            var tables = _context.RestaurantTables
                .Select(t => new
                {
                    t.RestaurantTableId,
                    Display = "Masa " + t.TableNumber + " - " + t.Zone + " - " + t.Seats + " locuri"
                })
                .ToList();

            ViewData["RestaurantTableId"] = new SelectList(tables, "RestaurantTableId", "Display");

            return View();
        }

        // POST: Reservations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReservationId,ReservationDateTime,Persons,Status,ClientId,RestaurantTableId")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["ClientId"] = new SelectList(_context.Clients, "ClientId", "FullName", reservation.ClientId);

            var tables = _context.RestaurantTables
                .Select(t => new
                {
                    t.RestaurantTableId,
                    Display = "Masa " + t.TableNumber + " - " + t.Zone + " - " + t.Seats + " locuri"
                })
                .ToList();

            ViewData["RestaurantTableId"] = new SelectList(tables, "RestaurantTableId", "Display", reservation.RestaurantTableId);

            return View(reservation);
        }

        // GET: Reservations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }

            ViewData["ClientId"] = new SelectList(_context.Clients, "ClientId", "FullName", reservation.ClientId);

            var tables = _context.RestaurantTables
                .Select(t => new
                {
                    t.RestaurantTableId,
                    Display = "Masa " + t.TableNumber + " - " + t.Zone + " - " + t.Seats + " locuri"
                })
                .ToList();

            ViewData["RestaurantTableId"] = new SelectList(tables, "RestaurantTableId", "Display", reservation.RestaurantTableId);

            return View(reservation);
        }

        // POST: Reservations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReservationId,ReservationDateTime,Persons,Status,ClientId,RestaurantTableId")] Reservation reservation)
        {
            if (id != reservation.ReservationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.ReservationId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["ClientId"] = new SelectList(_context.Clients, "ClientId", "FullName", reservation.ClientId);

            var tables = _context.RestaurantTables
                .Select(t => new
                {
                    t.RestaurantTableId,
                    Display = "Masa " + t.TableNumber + " - " + t.Zone + " - " + t.Seats + " locuri"
                })
                .ToList();

            ViewData["RestaurantTableId"] = new SelectList(tables, "RestaurantTableId", "Display", reservation.RestaurantTableId);

            return View(reservation);
        }

        // GET: Reservations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .AsNoTracking()
                .Include(r => r.Client)
                .Include(r => r.RestaurantTable)
                .FirstOrDefaultAsync(m => m.ReservationId == id);

            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }

            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservations.Any(e => e.ReservationId == id);
        }
    }
}
