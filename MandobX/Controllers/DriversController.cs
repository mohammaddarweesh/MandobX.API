using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MandobX.API.Authentication;
using MandobX.API.Data;
using MandobX.API.Models;

namespace MandobX.Controllers
{
    public class DriversController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DriversController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Drivers
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Drivers.Include(d => d.User).Include(d => d.Vehicle);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Drivers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var driver = await _context.Drivers
                .Include(d => d.User)
                .Include(d => d.Vehicle)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (driver == null)
            {
                return NotFound();
            }

            return View(driver);
        }

        // GET: Drivers/Create
        //public IActionResult Create()
        //{
        //    ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "UserName");
        //    ViewData["VehicleId"] = new SelectList(_context.Vehicles, "Id", "Number");
        //    return View();
        //}

        // POST: Drivers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Points,VehicleId,UserId")] Driver driver)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(driver);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "UserName", driver.UserId);
        //    ViewData["VehicleId"] = new SelectList(_context.Vehicles, "Id", "Number", driver.VehicleId);
        //    return View(driver);
        //}

        // GET: Drivers/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var driver = await _context.Drivers.FindAsync(id);
            if (driver == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "UserName", driver.UserId);
            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "Id", "Number", driver.VehicleId);
            return View(driver);
        }

        // POST: Drivers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Points,VehicleId,UserId")] Driver driver)
        {
            if (id != driver.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(driver);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DriverExists(driver.Id))
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
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "UserName", driver.UserId);
            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "Id", "Number", driver.VehicleId);
            return View(driver);
        }

        // GET: Drivers/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driver = await _context.Drivers
                .Include(d => d.User)
                .Include(d => d.Vehicle)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (driver == null)
            {
                return NotFound();
            }

            return View(driver);
        }

        // POST: Drivers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var driver = await _context.Drivers.FindAsync(id);
            _context.Drivers.Remove(driver);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Block(string id)
        {
            var driver = _context.Drivers.Include(d=>d.User).FirstOrDefault(d => d.Id == id);
            if (driver==null)
            {
                return NotFound();
            }
            driver.User.UserStatus = UserStatus.Blocked;
            _context.Drivers.Update(driver);
            await _context.SaveChangesAsync();

            return Ok(new Response { Code = "200", Data = null, Msg = "Driver was Blocked Successfuly", Status = "1" });
        }
        [HttpGet]
        public async Task<IActionResult> UnBlock(string id)
        {
            var driver = _context.Drivers.Include(d => d.User).FirstOrDefault(d => d.Id == id);
            if (driver == null)
            {
                return NotFound();
            }
            driver.User.UserStatus = UserStatus.Active;
            _context.Drivers.Update(driver);
            await _context.SaveChangesAsync();

            return Ok(new Response { Code = "200", Data = null, Msg = "Driver was UnBlocked Successfuly", Status = "1" });
        }
        private bool DriverExists(string id)
        {
            return _context.Drivers.Any(e => e.Id == id);
        }
    }
}
