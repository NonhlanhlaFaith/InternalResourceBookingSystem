using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InternalResourceBookingSystem.Data;
using InternalResourceBookingSystem.Models;
using System.Linq;
using System.Threading.Tasks;

namespace InternalResourceBookingSystem.Controllers
{
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Bookings
        public async Task<IActionResult> Index()
        {
            var bookings = await _context.Bookings
                .Include(b => b.Resource)
                .OrderByDescending(b => b.StartTime)
                .ToListAsync();

            return View(bookings);
        }

        // GET: Bookings/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Resources = await _context.Resources
                .Where(r => r.IsAvailable)
                .Select(r => new SelectListItem
                {
                    Value = r.Id.ToString(),
                    Text = r.Name
                }).ToListAsync();

            return View();
        }

        // POST: Bookings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Booking booking)
        {
            if (booking.EndTime <= booking.StartTime)
                ModelState.AddModelError("", "End time must be after start time.");

            bool conflict = await _context.Bookings.AnyAsync(b =>
                b.ResourceId == booking.ResourceId &&
                (
                    (booking.StartTime >= b.StartTime && booking.StartTime < b.EndTime) ||
                    (booking.EndTime > b.StartTime && booking.EndTime <= b.EndTime) ||
                    (booking.StartTime <= b.StartTime && booking.EndTime >= b.EndTime)
                )
            );

            if (conflict)
                ModelState.AddModelError("", "This resource is already booked during the requested time.");

            if (!ModelState.IsValid)
            {
                ViewBag.Resources = await _context.Resources
                    .Where(r => r.IsAvailable)
                    .Select(r => new SelectListItem
                    {
                        Value = r.Id.ToString(),
                        Text = r.Name
                    }).ToListAsync();

                return View(booking);
            }

            _context.Add(booking);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Bookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();

            ViewBag.Resources = await _context.Resources
                .Where(r => r.IsAvailable || r.Id == booking.ResourceId)
                .Select(r => new SelectListItem
                {
                    Value = r.Id.ToString(),
                    Text = r.Name,
                    Selected = r.Id == booking.ResourceId
                }).ToListAsync();

            return View(booking);
        }

        // POST: Bookings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Booking booking)
        {
            if (id != booking.Id) return NotFound();

            if (booking.EndTime <= booking.StartTime)
                ModelState.AddModelError("", "End time must be after start time.");

            bool conflict = await _context.Bookings.AnyAsync(b =>
                b.Id != booking.Id &&
                b.ResourceId == booking.ResourceId &&
                (
                    (booking.StartTime >= b.StartTime && booking.StartTime < b.EndTime) ||
                    (booking.EndTime > b.StartTime && booking.EndTime <= b.EndTime) ||
                    (booking.StartTime <= b.StartTime && booking.EndTime >= b.EndTime)
                )
            );

            if (conflict)
                ModelState.AddModelError("", "This resource is already booked during the requested time.");

            if (!ModelState.IsValid)
            {
                ViewBag.Resources = await _context.Resources
                    .Where(r => r.IsAvailable || r.Id == booking.ResourceId)
                    .Select(r => new SelectListItem
                    {
                        Value = r.Id.ToString(),
                        Text = r.Name,
                        Selected = r.Id == booking.ResourceId
                    }).ToListAsync();

                return View(booking);
            }

            try
            {
                _context.Update(booking);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Bookings.AnyAsync(b => b.Id == booking.Id))
                    return NotFound();
                else
                    throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings
                .Include(b => b.Resource)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null) return NotFound();

            return View(booking);
        }

        // POST: Bookings/Delete/5
        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
