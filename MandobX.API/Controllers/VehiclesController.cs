using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MandobX.API.Data;
using MandobX.API.Models;
using MandobX.API.Authentication;
using MandobX.API.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace MandobX.API.Controllers
{
    /// <summary>
    /// Vehicles Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="mapper"></param>
        /// <param name="userManager"></param>
        public VehiclesController(ApplicationDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        /// <summary>
        /// Get vehicles using Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/Vehicles/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVehicle(string id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);

            if (vehicle == null)
            {
                return NotFound(new Response { Code = "200", Data = null, Status = "0", Msg="Vehicles was not found" });
            }

            return Ok(new Response { Code = "200", Data = vehicle, Status = "1" });
        }

        // PUT: api/Vehicles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Update vehicle
        /// </summary>
        /// <param name="id"></param>
        /// <param name="vehicle"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVehicle(string id, Vehicle vehicle)
        {
            if (id != vehicle.Id)
            {
                return BadRequest();
            }

            _context.Entry(vehicle).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VehicleExists(id))
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

        // POST: api/Vehicles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Add vehicle
        /// </summary>
        /// <param name="vehicle"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PostVehicle(string userID, CreateVehicleViewModel vehicle)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userID);
                if (user!=null)
                {
                    var storedVehicle = _mapper.Map<Vehicle>(vehicle);
                    _context.Vehicles.Add(storedVehicle);
                    Driver driver = _context.Drivers.FirstOrDefault(d => d.UserId == userID);
                    driver.VehicleId = storedVehicle.Id;
                    _context.Drivers.Update(driver);
                    await _context.SaveChangesAsync();
                    return Ok(new Response { Msg = "Vehicle was created successfuly", Status = "1", Data = null, Code = "200" });
                }
                return NotFound(new Response { Msg = "user not found", Code = "500", Data = null, Status = "0" });
            }
            catch (Exception e)
            {
                string message = e.Message;
                if (e.InnerException!=null)
                {
                    message = message + " with internal exception message " + e.InnerException.Message;
                }
                Response response = new Response {
                    Msg = message,
                    Status = "1",
                    Data = null,
                    Code = "200"
                };
                return BadRequest(response);
            }
        }

        // DELETE: api/Vehicles/5
        /// <summary>
        /// Delete vehicle
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicle(string id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }

            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();
            return Ok(new Response { Code = "200", Data = null, Status = "1", Msg = "Vehicle was deleted successfuly" });
        }

        /// <summary>
        /// init create for vehicle (get car types and car brands)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Init()
        {
            var carbrands = await _context.CarBrands.ToListAsync();
            var cartypes = await _context.CarTypes.ToListAsync();
            return Ok(new Response { Code = "200", Data = new { cartypes = cartypes, carbrands = carbrands }, Msg = "", Status = "1" });
        }

        private bool VehicleExists(string id)
        {
            return _context.Vehicles.Any(e => e.Id == id);
        }
    }
}
