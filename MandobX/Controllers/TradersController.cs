using System;
using System.Collections.Generic;
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
    public class TradersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TradersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Traders
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Traders.Include(t => t.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Traders/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var trader = await _context.Traders
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trader == null)
            {
                return NotFound();
            }

            return View(trader);
        }

        // GET: Traders/Create
        //public IActionResult Create()
        //{
        //    ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id");
        //    return View();
        //}

        // POST: Traders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Points,UserId")] Trader trader)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(trader);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "UserName", trader.UserId);
        //    return View(trader);
        //}

        // GET: Traders/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var trader = await _context.Traders.FindAsync(id);
            if (trader == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "UserName", trader.UserId);
            return View(trader);
        }

        // POST: Traders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Points,UserId")] Trader trader)
        {
            if (id != trader.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trader);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TraderExists(trader.Id))
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
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "UserName", trader.UserId);
            return View(trader);
        }

        // GET: Traders/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var trader = await _context.Traders
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trader == null)
            {
                return NotFound();
            }

            return View(trader);
        }

        // POST: Traders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var trader = await _context.Traders.FindAsync(id);
            _context.Traders.Remove(trader);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Block(string id)
        {
            var trader = _context.Traders.Include(t => t.User).FirstOrDefault(t => t.Id==id);

            if (trader == null)
            {
                return NotFound();
            }
            trader.User.UserStatus = UserStatus.Blocked;
            _context.Traders.Update(trader);
            await _context.SaveChangesAsync();

            return RedirectToAction("index");
        }
        [HttpGet]
        public async Task<IActionResult> UnBlock(string id)
        {
            var trader = _context.Traders.Include(t => t.User).FirstOrDefault(t => t.Id == id);
            if (trader == null)
            {
                return NotFound();
            }
            trader.User.UserStatus = UserStatus.Active;
            _context.Traders.Update(trader);
            await _context.SaveChangesAsync();

            return RedirectToAction("index");
        }
        private bool TraderExists(string id)
        {
            return _context.Traders.Any(e => e.Id == id);
        }
    }
}
