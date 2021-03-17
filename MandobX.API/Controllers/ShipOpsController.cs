using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MandobX.API.Data;
using MandobX.API.Models;
using MandobX.API.ViewModels;
using MandobX.API.Authentication;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace MandobX.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShipOpsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> userManager;

        public ShipOpsController(IMapper mapper, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            this.userManager = userManager;
        }

        //Get lists of drivers, regions and packagges type to create shipment operation
        /// <summary>
        /// all shipment indexes for adding new shipment
        /// </summary>
        /// <returns>Response</returns>
        [Route("Init")]
        [HttpGet]
        public async Task<IActionResult> InitShipment()
        {
            ShipmentViewModel shipmentInitViewModel = new ShipmentViewModel
            {
                Drivers = await _context.Drivers.Include(d => d.User).Include(d => d.Vehicle).ToListAsync(),
                PackageTypes = await _context.PackageTypes.ToListAsync(),
                Regions = await _context.Regions.ToListAsync()
            };
            return Ok(new Response { Code = "200", Data = shipmentInitViewModel, Msg = "Task Completed Succesfully", Status = "1" });
        }
        // GET: api/ShipmentOperations
        /// <summary>
        /// return all shipment operation related to driver or trader
        /// </summary>
        /// <returns></returns>
        [Route("List")]
        [HttpGet]
        public async Task<IActionResult> GetShipmentOperations()
        {
            var shipments = new List<ShipmentOperation>();
            var shipmentViewModels = new List<ShipmentListViewModel>();
            string userId = ApplicationUserId().ToString();
            if (User.Claims.Any(d => d.Value == "Admin"))
            {
                shipments = await _context.ShipmentOperations.Include(s => s.FromRegion)
                                                             .Include(s => s.Driver.User)
                                                             .Include(s => s.PackageType)
                                                             .Include(s => s.ToRegion).ToListAsync();
                shipmentViewModels = _mapper.Map<List<ShipmentListViewModel>>(shipments);
                return Ok(new Response { Code = "200", Data = shipmentViewModels, Msg = "Success", Status = "1" });
            }
            else
            {
                if (User.IsInRole("Driver"))
                {
                    shipments = await _context.ShipmentOperations.Include(s => s.FromRegion)
                                                                 .Include(s => s.Driver.User)
                                                                 .Include(s => s.PackageType)
                                                                 .Include(s => s.ToRegion)
                                                                 .Where(t => t.DriverId == userId).ToListAsync();
                    shipmentViewModels = _mapper.Map<List<ShipmentListViewModel>>(shipments);
                    return Ok(new Response { Code = "200", Data = shipmentViewModels, Msg = "Success", Status = "1" });
                }
                else if (User.IsInRole("Trader"))
                {
                    shipments = await _context.ShipmentOperations.Include(s => s.FromRegion)
                                                                 .Include(s => s.Driver.User)
                                                                 .Include(s => s.PackageType)
                                                                 .Include(s => s.ToRegion)
                                                                 .Where(t => t.DriverId == userId).ToListAsync();
                    shipmentViewModels = _mapper.Map<List<ShipmentListViewModel>>(shipments);
                    return Ok(new Response { Code = "200", Data = shipmentViewModels, Msg = "Success", Status = "1" });
                }
                return NotFound();
            }
        }

        // GET: api/ShipmentOperations/5
        [Route("Details/{id}")]
        [HttpGet]
        public async Task<ActionResult<ShipmentOperation>> GetShipmentOperation(string id)
        {
            var shipmentOperation = await _context.ShipmentOperations.FindAsync(id);

            if (shipmentOperation == null)
            {
                return NotFound();
            }

            return shipmentOperation;
        }

        // PUT: api/ShipmentOperations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutShipmentOperation(string id, ShipmentOperation shipmentOperation)
        {
            if (id != shipmentOperation.Id)
            {
                return BadRequest();
            }

            _context.Entry(shipmentOperation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShipmentOperationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ShipmentOperations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostShipmentOperation(CreateShipmentViewModel shipmentOperationViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    GoogleMap googleMap = _mapper.Map<GoogleMap>(shipmentOperationViewModel);
                    _context.GoogleMaps.Add(googleMap);
                    await _context.SaveChangesAsync();
                    ShipmentOperation shipmentOperation = _mapper.Map<ShipmentOperation>(shipmentOperationViewModel);
                    shipmentOperation.GoogleMapId = googleMap.Id;
                    shipmentOperation.CreationDate = DateTime.Now.ToString();
                    _context.ShipmentOperations.Add(shipmentOperation);
                    await _context.SaveChangesAsync();
                    return Ok(new Response { Code = "200", Data = null, Msg = "Shipment Operation Created Successfuly", Status = "1" });
                }
                else
                {
                    return BadRequest(new Response { Code = "500", Data = null, Msg = "Please fill all the required fields", Status = "0" });
                }
            }
            catch (Exception e)
            {
                return BadRequest(new Response { Code = "500", Data = null, Msg = string.Format("{0} and internal exception is {1}", e.Message, e.InnerException == null ? "nothing" : e.InnerException.Message), Status = "0" });
            }
        }

        // DELETE: api/ShipmentOperations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShipmentOperation(string id)
        {
            var shipmentOperation = await _context.ShipmentOperations.FindAsync(id);
            if (shipmentOperation == null)
            {
                return NotFound();
            }

            _context.ShipmentOperations.Remove(shipmentOperation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ShipmentOperationExists(string id)
        {
            return _context.ShipmentOperations.Any(e => e.Id == id);
        }

        private async Task<string> ApplicationUserId()
        {
            var user = await userManager.FindByNameAsync(User?.Identity.Name);
            if (user != null)
            {
                if (User.IsInRole(UserRoles.Admin))
                    return user.Id;
                else if (User.IsInRole(UserRoles.Trader))
                {
                    var trader = _context.Traders.FirstOrDefault(t => t.UserId == user.Id);
                    return trader.Id;
                }
                else if (User.IsInRole(UserRoles.Driver))
                {
                    var driver = _context.Drivers.FirstOrDefault(d => d.UserId == user.Id);
                    return driver.Id;
                }
            }
            else
            {
                return "User Not Found";
            }
            return "";
        }
    }
}
