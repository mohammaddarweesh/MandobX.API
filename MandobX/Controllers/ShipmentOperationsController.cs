using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MandobX.API.Data;
using MandobX.API.Models;
using AutoMapper;
using MandobX.API.ViewModels;
using System.Collections.Generic;

namespace MandobX.Controllers
{
    //[Authorize]
    public class ShipmentOperationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ShipmentOperationsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: ShipmentOperations
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ShipmentOperations.Include(s => s.FromRegion).Include(s => s.GoogleMap).Include(s => s.PackageType).Include(s => s.ToRegion).Include(s=>s.Driver.User).Include(s=>s.Trader.User);
            var shipmentoperations = await applicationDbContext.ToListAsync();
            var shippedfinal = _mapper.Map<List<ShipmentOperationViewModel>>(shipmentoperations);
            return View(shippedfinal);
        }

        // GET: ShipmentOperations/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var shipmentOperation = await _context.ShipmentOperations
                .Include(s => s.FromRegion)
                .Include(s => s.GoogleMap)
                .Include(s => s.PackageType)
                .Include(s => s.ToRegion)
                .Include(s => s.Driver.User)
                .Include(s => s.Trader.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            ShipmentOperationViewModel shipmentOperationViewModel = _mapper.Map<ShipmentOperationViewModel>(shipmentOperation);
            if (shipmentOperation == null)
            {
                return NotFound();
            }

            return View(shipmentOperationViewModel);
        }

        // GET: ShipmentOperations/Create
        public IActionResult Create()
        {
            ViewData["FromRegionId"] = new SelectList(_context.Regions, "Id", "Name");
            ViewData["GoogleMapId"] = new SelectList(_context.GoogleMaps, "Id", "Id");
            ViewData["PackageTypeId"] = new SelectList(_context.PackageTypes, "Id", "Name");
            ViewData["ToRegionId"] = new SelectList(_context.Regions, "Id", "Name");
            return View();
        }

        // POST: ShipmentOperations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FromRegionId,ToRegionId,GoogleMapId,CreationDate,ShipmentDate,DriverId,TraderId,RecieverPhoneNumber,RecieverName,Packages,PackageTypeId,Price,ShipmentStatus,ToTraderCode,ToRecieverCode")] ShipmentOperation shipmentOperation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shipmentOperation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FromRegionId"] = new SelectList(_context.Regions, "Id", "Name", shipmentOperation.FromRegionId);
            ViewData["GoogleMapId"] = new SelectList(_context.GoogleMaps, "Id", "Id", shipmentOperation.GoogleMapId);
            ViewData["PackageTypeId"] = new SelectList(_context.PackageTypes, "Id", "Name", shipmentOperation.PackageTypeId);
            ViewData["ToRegionId"] = new SelectList(_context.Regions, "Id", "Name", shipmentOperation.ToRegionId);
            return View(shipmentOperation);
        }

        // GET: ShipmentOperations/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var shipmentOperation = await _context.ShipmentOperations.FindAsync(id);
            if (shipmentOperation == null)
            {
                return NotFound();
            }
            ViewData["FromRegionId"] = new SelectList(_context.Regions, "Id", "Name", shipmentOperation.FromRegionId);
            ViewData["GoogleMapId"] = new SelectList(_context.GoogleMaps, "Id", "Id", shipmentOperation.GoogleMapId);
            ViewData["PackageTypeId"] = new SelectList(_context.PackageTypes, "Id", "Name", shipmentOperation.PackageTypeId);
            ViewData["ToRegionId"] = new SelectList(_context.Regions, "Id", "Name", shipmentOperation.ToRegionId);
            return View(shipmentOperation);
        }

        // POST: ShipmentOperations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,FromRegionId,ToRegionId,GoogleMapId,CreationDate,ShipmentDate,DriverId,TraderId,RecieverPhoneNumber,RecieverName,Packages,PackageTypeId,Price,ShipmentStatus,ToTraderCode,ToRecieverCode")] ShipmentOperation shipmentOperation)
        {
            if (id != shipmentOperation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    ShipmentOperation shipment = _context.ShipmentOperations.Find(id);
                    shipment.Price = shipmentOperation.Price;
                    shipment.ShipmentStatus = ShipmentStatus.AdminAccepted;
                    _context.Update(shipment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShipmentOperationExists(shipmentOperation.Id))
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
            ViewData["FromRegionId"] = new SelectList(_context.Regions, "Id", "Name", shipmentOperation.FromRegionId);
            ViewData["GoogleMapId"] = new SelectList(_context.GoogleMaps, "Id", "Id", shipmentOperation.GoogleMapId);
            ViewData["PackageTypeId"] = new SelectList(_context.PackageTypes, "Id", "Name", shipmentOperation.PackageTypeId);
            ViewData["ToRegionId"] = new SelectList(_context.Regions, "Id", "Name", shipmentOperation.ToRegionId);
            return View(shipmentOperation);
        }

        // GET: ShipmentOperations/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var shipmentOperation = await _context.ShipmentOperations
                .Include(s => s.FromRegion)
                .Include(s => s.GoogleMap)
                .Include(s => s.PackageType)
                .Include(s => s.ToRegion)
                .Include(s => s.Driver.User)
                .Include(s => s.Trader.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shipmentOperation == null)
            {
                return NotFound();
            }
            ShipmentOperationViewModel shipment = _mapper.Map<ShipmentOperationViewModel>(shipmentOperation);

            return View(shipment);
        }

        // POST: ShipmentOperations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var shipmentOperation = await _context.ShipmentOperations.FindAsync(id);
            _context.ShipmentOperations.Remove(shipmentOperation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> AcceptShipment(string id)
        {
            try
            {
                var shipment = _context.ShipmentOperations.Find(id);
                if (shipment==null)
                {
                    return NotFound();
                }
                shipment.ShipmentStatus = ShipmentStatus.AdminAccepted;
                _context.ShipmentOperations.Update(shipment);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details",new { id });
            }
            catch (System.Exception)
            {
                return NotFound();
            }
        }

        [HttpGet]
        public async Task<IActionResult> RejectShipment(string id)
        {
            try
            {
                var shipment = _context.ShipmentOperations.Find(id);
                if (shipment == null)
                {
                    return NotFound();
                }
                shipment.ShipmentStatus = ShipmentStatus.AdminRejected;
                _context.ShipmentOperations.Update(shipment);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { id });
            }
            catch (System.Exception)
            {
                return NotFound();
            }
        }

        private bool ShipmentOperationExists(string id)
        {
            return _context.ShipmentOperations.Any(e => e.Id == id);
        }
    }
}
