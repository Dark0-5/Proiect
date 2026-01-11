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
    public class OrdersController : Controller
    {
        private readonly AppDbContext _context;

        public OrdersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Orders
                .AsNoTracking()
                .Include(o => o.Reservation);

            return View(await appDbContext.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .AsNoTracking()
                .Include(o => o.Reservation)
                .FirstOrDefaultAsync(m => m.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            // ✅ dropdown frumos: Data + Client + Masa
            var reservations = _context.Reservations
                .Include(r => r.Client)
                .Include(r => r.RestaurantTable)
                .Select(r => new
                {
                    r.ReservationId,
                    Display = r.ReservationDateTime.ToString("yyyy-MM-dd HH:mm")
                              + " - " + r.Client.FullName
                              + " - Masa " + r.RestaurantTable.TableNumber
                })
                .ToList();

            ViewData["ReservationId"] = new SelectList(reservations, "ReservationId", "Display");
            return View();
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,OrderDateTime,Status,ReservationId")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // ✅ repopulare dropdown frumos daca ModelState nu e valid
            var reservations = _context.Reservations
                .Include(r => r.Client)
                .Include(r => r.RestaurantTable)
                .Select(r => new
                {
                    r.ReservationId,
                    Display = r.ReservationDateTime.ToString("yyyy-MM-dd HH:mm")
                              + " - " + r.Client.FullName
                              + " - Masa " + r.RestaurantTable.TableNumber
                })
                .ToList();

            ViewData["ReservationId"] = new SelectList(reservations, "ReservationId", "Display", order.ReservationId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            // ✅ dropdown frumos
            var reservations = _context.Reservations
                .Include(r => r.Client)
                .Include(r => r.RestaurantTable)
                .Select(r => new
                {
                    r.ReservationId,
                    Display = r.ReservationDateTime.ToString("yyyy-MM-dd HH:mm")
                              + " - " + r.Client.FullName
                              + " - Masa " + r.RestaurantTable.TableNumber
                })
                .ToList();

            ViewData["ReservationId"] = new SelectList(reservations, "ReservationId", "Display", order.ReservationId);
            return View(order);
        }

        // POST: Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,OrderDateTime,Status,ReservationId")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
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

            // ✅ repopulare dropdown frumos daca ModelState nu e valid
            var reservations = _context.Reservations
                .Include(r => r.Client)
                .Include(r => r.RestaurantTable)
                .Select(r => new
                {
                    r.ReservationId,
                    Display = r.ReservationDateTime.ToString("yyyy-MM-dd HH:mm")
                              + " - " + r.Client.FullName
                              + " - Masa " + r.RestaurantTable.TableNumber
                })
                .ToList();

            ViewData["ReservationId"] = new SelectList(reservations, "ReservationId", "Display", order.ReservationId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .AsNoTracking()
                .Include(o => o.Reservation)
                .FirstOrDefaultAsync(m => m.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}
