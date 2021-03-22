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
    /// <summary>
    /// shipment controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ShipOpsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> userManager;
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="context"></param>
        /// <param name="userManager"></param>
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
                Drivers = await _context.Drivers.Where(d=>d.User.UserStatus == UserStatus.Active).Include(d => d.User).Include(d => d.Vehicle).ToListAsync(),
                PackageTypes = await _context.PackageTypes.ToListAsync(),
                Regions = await _context.Regions.ToListAsync()
            };
            return Ok(new Response { Code = "200", Data = shipmentInitViewModel, Msg = "Task Completed Succesfully", Status = "1" });
        }
        // GET: api/ShipmentOperations
        /// <summary>
        /// return all shipment operation related to driver or trader
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        [Route("List/{userid}")]
        [HttpGet]
        public async Task<IActionResult> GetShipmentOperations(string userid)
        {
            var shipments = new List<ShipmentOperation>();
            var shipmentViewModels = new List<ShipmentListViewModel>();
            var user = await userManager.FindByIdAsync(userid);
            if (user.UserType == UserRoles.Driver)
            {
                var driver = _context.Drivers.FirstOrDefault(d => d.UserId == userid);
                shipments = await _context.ShipmentOperations.Include(s => s.FromRegion)
                                                             .Include(s => s.Driver.User)
                                                             .Include(s => s.PackageType)
                                                             .Include(s => s.ToRegion)
                                                             .Where(t => t.DriverId == driver.Id).ToListAsync();
                shipmentViewModels = _mapper.Map<List<ShipmentListViewModel>>(shipments);
                return Ok(new Response { Code = "200", Data = shipmentViewModels, Msg = "Success", Status = "1" });
            }
            else if (user.UserType == UserRoles.Trader)
            {
                var trader = _context.Traders.FirstOrDefault(d => d.UserId == userid);
                shipments = await _context.ShipmentOperations.Include(s => s.FromRegion)
                                                             .Include(s => s.Driver.User)
                                                             .Include(s => s.PackageType)
                                                             .Include(s => s.ToRegion)
                                                             .Where(t => t.TraderId == trader.Id).ToListAsync();
                shipmentViewModels = _mapper.Map<List<ShipmentListViewModel>>(shipments);
                return Ok(new Response { Code = "200", Data = shipmentViewModels, Msg = "Success", Status = "1" });
            }
            return NotFound();
        }

        // GET: api/ShipmentOperations/5
        /// <summary>
        /// details of specific shipment operation
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("Details/{id}")]
        [HttpGet]
        public async Task<ActionResult<ShipmentOperation>> GetShipmentOperation(string id)
        {
            try
            {
                var shipmentOperation = await _context.ShipmentOperations.FindAsync(id);

                if (shipmentOperation == null)
                {
                    return NotFound();
                }

                return Ok(new Response { Data = shipmentOperation, Code = "200", Msg = "Success", Status = "1" });
            }
            catch (Exception e)
            {

                return BadRequest(new Response { Code = "500", Data = null, Msg = string.Format("{0} and internal exception is {1}", e.Message, e.InnerException == null ? "nothing" : e.InnerException.Message), Status = "0" });
            }
        }

        // PUT: api/ShipmentOperations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// update shipment operation
        /// </summary>
        /// <param name="id"></param>
        /// <param name="editShipmentViewModel"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutShipmentOperation(string id, EditShipmentViewModel editShipmentViewModel)
        {
            ShipmentOperation shipmentOperation = _mapper.Map<ShipmentOperation>(editShipmentViewModel);
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
                    return BadRequest(new Response { Code = "500", Data = null, Msg = "something went wrong", Status = "0" });
                }
            }

            return Ok(new Response { Code = "200", Data = null, Msg = "Shipment updated successfully", Status = "1" });
        }

        // POST: api/ShipmentOperations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// add shpment operation
        /// </summary>
        /// <param name="shipmentOperationViewModel"></param>
        /// <returns></returns>
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
        /// <summary>
        /// delete shipment operation
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
    }
}
