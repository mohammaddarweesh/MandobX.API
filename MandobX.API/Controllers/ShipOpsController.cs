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
using Microsoft.AspNetCore.Authorization;
using MandobX.API.Services.IService;

namespace MandobX.API.Controllers
{
    /// <summary>
    /// shipment controller
    /// </summary>
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ShipOpsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMessageService _messageService;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="context"></param>
        /// <param name="userManager"></param>
        /// <param name="messageService"></param>
        public ShipOpsController(IMapper mapper, ApplicationDbContext context, UserManager<ApplicationUser> userManager, IMessageService messageService)
        {
            _context = context;
            _mapper = mapper;
            this.userManager = userManager;
            _messageService = messageService;
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
            try
            {
                var drivers = _mapper.Map<List<DriverCreateShipmentViewModel>>(await _context.Drivers.Where(d => d.User.UserStatus == UserStatus.Active).Include(d => d.Vehicle.CarBrand).Include(d => d.Vehicle.CarType).Include(d => d.User).Include(d => d.Vehicle).ToListAsync());
                ShipmentViewModel shipmentInitViewModel = new ShipmentViewModel
                {
                    Drivers = drivers,
                    PackageTypes = await _context.PackageTypes.ToListAsync(),
                    Regions = await _context.Regions.ToListAsync()
                };
                return Ok(new Response { Code = "200", Data = shipmentInitViewModel, Msg = "Task Completed Succesfully", Status = "1" });
            }
            catch (Exception e)
            {

                return BadRequest(new Response { Code = "500", Data = null, Msg = string.Format("{0} and internal exception is {1}", e.Message, e.InnerException == null ? "nothing" : e.InnerException.Message), Status = "0" });
            }
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
                                                             .Include(s => s.Trader.User)
                                                             .Include(s => s.PackageType)
                                                             .Include(s => s.ToRegion)
                                                             .Include(s => s.GoogleMap)
                                                             .Where(t => t.DriverId == driver.Id && t.ShipmentStatus != ShipmentStatus.DriverRejected && t.ShipmentStatus != ShipmentStatus.AdminRejected && t.ShipmentStatus != ShipmentStatus.Pending && (t.Price != 0 || t.ShipmentStatus == ShipmentStatus.DriverRequested)).ToListAsync();
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
                var shipmentOperation = await _context.ShipmentOperations
                    .Include(s => s.Driver)
                    .Include(s => s.FromRegion)
                    .Include(s => s.GoogleMap)
                    .Include(s => s.PackageType)
                    .Include(s => s.ToRegion)
                    .Include(s => s.Trader)
                    .FirstOrDefaultAsync(s => s.Id == id);

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

        // GET: api/ShipmentOperations/5
        /// <summary>
        /// details of specific shipment operation
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("GetEdit/{id}")]
        [HttpGet]
        public async Task<ActionResult<ShipmentOperation>> GetShipmentOperationForEdit(string id)
        {
            try
            {
                var shipmentOperation = _context.ShipmentOperations
                                                      .Include(s => s.Driver)
                                                      .FirstOrDefault(s => s.Id == id);
                if (shipmentOperation == null)
                {
                    return NotFound();
                }
                var editShipmentViewModel = _mapper.Map<EditShipmentViewModel>(shipmentOperation);
                var drivers = _mapper.Map<List<DriverCreateShipmentViewModel>>(await _context.Drivers.Where(d => d.User.UserStatus == UserStatus.Active).Include(d => d.Vehicle.CarBrand).Include(d => d.Vehicle.CarType).Include(d => d.User).Include(d => d.Vehicle).ToListAsync());
                ShipmentViewModel shipmentInitViewModel = new ShipmentViewModel
                {
                    Drivers = drivers,
                    PackageTypes = await _context.PackageTypes.ToListAsync(),
                    Regions = await _context.Regions.ToListAsync()
                };
                return Ok(new Response { Data = new { shipment = editShipmentViewModel, shipmentInitViewModel = shipmentInitViewModel }, Code = "200", Msg = "Success", Status = "1" });
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
                    var trader = await _context.Traders.FirstOrDefaultAsync(t => t.UserId == shipmentOperationViewModel.TraderId);
                    var driver = await _context.Drivers.FirstOrDefaultAsync(d => d.UserId == shipmentOperationViewModel.DriverId);
                    shipmentOperationViewModel.TraderId = trader.Id;
                    shipmentOperationViewModel.DriverId = driver == null ? null : driver.Id;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shipmentId"></param>
        /// <param name="newStatus"></param>
        /// <param name="Code"></param>
        /// <returns></returns>
        [HttpPost("ChangeStatus")]
        public async Task<IActionResult> ChangeStatus(string shipmentId, int newStatus, string Code)
        {
            try
            {
                if (!string.IsNullOrEmpty(shipmentId))
                {
                    var shipment = await _context.ShipmentOperations.Include(s => s.Trader.User).FirstOrDefaultAsync(s => s.Id == shipmentId);
                    
                    if (shipment != null)
                    {
                        string phoneNumber = shipment.Trader.User.PhoneNumber;
                        if (newStatus == (int)ShipmentStatus.TraderAccepted)
                        {
                            int toTradercode = await _messageService.SendMessage(phoneNumber, "your + pick-up + Code + is +");
                            if (toTradercode == 0)
                            {
                                return BadRequest(new Response { Code = "500", Data = null, Msg = "Something went wrong please try again later", Status = "0" });
                            }
                            shipment.ToTraderCode = toTradercode;
                        }
                        else if(newStatus == (int)ShipmentStatus.OnTheWay)
                        {
                            if (int.Parse(Code) != shipment.ToTraderCode)
                            {
                                return BadRequest(new Response { Code = "500", Data = null, Msg = "Please provide the correct code we sent to the trader", Status = "0" });
                            }
                            int toRecievercode = await _messageService.SendMessage(shipment.RecieverPhoneNumber, "Your + Recieving + Code + is +");
                            if (toRecievercode == 0)
                            {
                                return BadRequest(new Response { Code = "500", Data = null, Msg = "Something went wrong please try again later", Status = "0" });
                            }
                            shipment.ToRecieverCode = toRecievercode;
                        }
                        else if(newStatus == (int)ShipmentStatus.Shipped)
                        {
                            if (int.Parse(Code) != shipment.ToRecieverCode)
                            {
                                return BadRequest(new Response { Code = "500", Data = null, Msg = "Please provide the correct code we sent to the reciever", Status = "0" });
                            }
                        }
                        shipment.ShipmentStatus = (ShipmentStatus)newStatus;
                        _context.ShipmentOperations.Update(shipment);
                        await _context.SaveChangesAsync();
                        return Ok(new Response { Code = "200", Data = null, Msg = "Task Completed Successfuly", Status = "1" });
                    }
                }
                return NotFound();
            }
            catch (Exception e)
            {
                return BadRequest(new Response { Code = "500", Data = null, Msg = string.Format("{0} and internal exception is {1}", e.Message, e.InnerException == null ? "nothing" : e.InnerException.Message), Status = "0" });
            }
        }
        /// <summary>
        /// driver apply for a shipment
        /// </summary>
        /// <param name="shipmentId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost("requestshipment")]
        public async Task<IActionResult> ApplyForShipment(string shipmentId, string userId)
        {
            var driver = await _context.Drivers.FirstOrDefaultAsync(d => d.UserId == userId);
            var shipment = await _context.ShipmentOperations.FindAsync(shipmentId);
            if (driver != null && shipment != null)
            {
                shipment.DriverId = driver.Id;
                shipment.ShipmentStatus = ShipmentStatus.DriverRequested;
                _context.ShipmentOperations.Update(shipment);
                await _context.SaveChangesAsync();
                return Ok(new Response { Code = "200", Data = null, Msg = "Apply for shipment was done successfuly", Status = "1" });
            }
            return NotFound();
        }

        private bool ShipmentOperationExists(string id)
        {
            return _context.ShipmentOperations.Any(e => e.Id == id);
        }
    }
}
