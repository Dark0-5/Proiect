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

    public class OrderItemsController : Controller
    {
        private readonly AppDbContext _context;

        public OrderItemsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: OrderItems
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.OrderItems
                .AsNoTracking()
                .Include(o => o.MenuItem)
                .Include(o => o.Order);

            return View(await appDbContext.ToListAsync());
        }

        // GET: OrderItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderItem = await _context.OrderItems
                .AsNoTracking()
                .Include(o => o.MenuItem)
                .Include(o => o.Order)
                .FirstOrDefaultAsync(m => m.OrderItemId == id);

            if (orderItem == null)
            {
                return NotFound();
            }

            return View(orderItem);
        }

        // GET: OrderItems/Create
        public IActionResult Create()
        {
            ViewData["MenuItemId"] = new SelectList(_context.MenuItems, "MenuItemId", "Name");

            // ✅ dropdown frumos pentru comanda: "Order #X - Status - Data"
            var orders = _context.Orders
                .Select(o => new
                {
                    o.OrderId,
                    Display = "Order #" + o.OrderId + " - " + o.Status + " - " + o.OrderDateTime.ToString("yyyy-MM-dd HH:mm")
                })
                .ToList();

            ViewData["OrderId"] = new SelectList(orders, "OrderId", "Display");

            return View();
        }

        // POST: OrderItems/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderItemId,OrderId,MenuItemId,Quantity,UnitPrice")] OrderItem orderItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["MenuItemId"] = new SelectList(_context.MenuItems, "MenuItemId", "Name", orderItem.MenuItemId);

            var orders = _context.Orders
                .Select(o => new
                {
                    o.OrderId,
                    Display = "Order #" + o.OrderId + " - " + o.Status + " - " + o.OrderDateTime.ToString("yyyy-MM-dd HH:mm")
                })
                .ToList();

            ViewData["OrderId"] = new SelectList(orders, "OrderId", "Display", orderItem.OrderId);

            return View(orderItem);
        }

        // GET: OrderItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderItem = await _context.OrderItems.FindAsync(id);
            if (orderItem == null)
            {
                return NotFound();
            }

            ViewData["MenuItemId"] = new SelectList(_context.MenuItems, "MenuItemId", "Name", orderItem.MenuItemId);

            var orders = _context.Orders
                .Select(o => new
                {
                    o.OrderId,
                    Display = "Order #" + o.OrderId + " - " + o.Status + " - " + o.OrderDateTime.ToString("yyyy-MM-dd HH:mm")
                })
                .ToList();

            ViewData["OrderId"] = new SelectList(orders, "OrderId", "Display", orderItem.OrderId);

            return View(orderItem);
        }

        // POST: OrderItems/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderItemId,OrderId,MenuItemId,Quantity,UnitPrice")] OrderItem orderItem)
        {
            if (id != orderItem.OrderItemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderItemExists(orderItem.OrderItemId))
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

            ViewData["MenuItemId"] = new SelectList(_context.MenuItems, "MenuItemId", "Name", orderItem.MenuItemId);

            var orders = _context.Orders
                .Select(o => new
                {
                    o.OrderId,
                    Display = "Order #" + o.OrderId + " - " + o.Status + " - " + o.OrderDateTime.ToString("yyyy-MM-dd HH:mm")
                })
                .ToList();

            ViewData["OrderId"] = new SelectList(orders, "OrderId", "Display", orderItem.OrderId);

            return View(orderItem);
        }

        // GET: OrderItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderItem = await _context.OrderItems
                .AsNoTracking()
                .Include(o => o.MenuItem)
                .Include(o => o.Order)
                .FirstOrDefaultAsync(m => m.OrderItemId == id);

            if (orderItem == null)
            {
                return NotFound();
            }

            return View(orderItem);
        }

        // POST: OrderItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderItem = await _context.OrderItems.FindAsync(id);
            if (orderItem == null)
            {
                return NotFound();
            }

            _context.OrderItems.Remove(orderItem);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool OrderItemExists(int id)
        {
            return _context.OrderItems.Any(e => e.OrderItemId == id);
        }
    }
}
