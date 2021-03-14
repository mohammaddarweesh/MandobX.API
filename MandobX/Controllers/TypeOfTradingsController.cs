using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MandobX.API.Authentication;
using MandobX.API.Data;

namespace MandobX.Controllers
{
    public class TypeOfTradingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TypeOfTradingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TypeOfTradings
        public async Task<IActionResult> Index()
        {
            return View(await _context.TypeOftradings.ToListAsync());
        }

        // GET: TypeOfTradings/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var typeOfTrading = await _context.TypeOftradings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (typeOfTrading == null)
            {
                return NotFound();
            }

            return View(typeOfTrading);
        }

        // GET: TypeOfTradings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TypeOfTradings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] TypeOfTrading typeOfTrading)
        {
            if (ModelState.IsValid)
            {
                _context.Add(typeOfTrading);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(typeOfTrading);
        }

        // GET: TypeOfTradings/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var typeOfTrading = await _context.TypeOftradings.FindAsync(id);
            if (typeOfTrading == null)
            {
                return NotFound();
            }
            return View(typeOfTrading);
        }

        // POST: TypeOfTradings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name")] TypeOfTrading typeOfTrading)
        {
            if (id != typeOfTrading.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(typeOfTrading);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TypeOfTradingExists(typeOfTrading.Id))
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
            return View(typeOfTrading);
        }

        // GET: TypeOfTradings/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var typeOfTrading = await _context.TypeOftradings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (typeOfTrading == null)
            {
                return NotFound();
            }

            return View(typeOfTrading);
        }

        // POST: TypeOfTradings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var typeOfTrading = await _context.TypeOftradings.FindAsync(id);
            _context.TypeOftradings.Remove(typeOfTrading);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TypeOfTradingExists(string id)
        {
            return _context.TypeOftradings.Any(e => e.Id == id);
        }
    }
}
