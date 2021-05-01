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
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;

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
        private readonly IWebHostEnvironment _environment;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="mapper"></param>
        /// <param name="userManager"></param>
        /// <param name="environment"></param>
        public VehiclesController(ApplicationDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager, IWebHostEnvironment environment)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _environment = environment;
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
            var vehicle = await _context.Vehicles.FirstOrDefaultAsync(v => v.Id == id);
            if (vehicle == null)
            {
                return NotFound(new Response { Code = "200", Data = null, Status = "0", Msg="Vehicles was not found" });
            }
            var driver = _context.Drivers.FirstOrDefault(d => d.VehicleId == id);
            var userId = driver.UserId;
            vehicle.UploadedFiles = _context.UploadedFiles.Where(u => u.UserId == userId && u.FileType == FileType.Vehicle).ToList();
            foreach (var uploadedFile in vehicle.UploadedFiles)
            {
                uploadedFile.FilePath = "http://mori23-001-site1.dtempurl.com/images/" + uploadedFile.FilePath;
            }
            return Ok(new Response { Code = "200", Data = vehicle, Status = "1" });
        }
        /// <summary>
        /// Get vehicles using Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/Vehicles/5
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> GetVehicleForEdit(string id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);

            if (vehicle == null)
            {
                return NotFound(new Response { Code = "200", Data = null, Status = "0", Msg="Vehicles was not found" });
            }
            var driver = _context.Drivers.FirstOrDefault(d => d.VehicleId == id);
            var userId = driver.UserId;
            vehicle.UploadedFiles = _context.UploadedFiles.Where(u => u.UserId == userId && u.FileType == FileType.Vehicle).ToList();
            var carbrands = await _context.CarBrands.ToListAsync();
            var cartypes = await _context.CarTypes.ToListAsync();
            foreach (var uploadedFile in vehicle.UploadedFiles)
            {
                uploadedFile.FilePath = "http://mori23-001-site1.dtempurl.com/images/" + uploadedFile.FilePath;
            }
            return Ok(new Response { Code = "200", Data = new { Vehicle = vehicle, CarBrands = carbrands, CarTypes = cartypes }, Status = "1" });
        }

        // PUT: api/Vehicles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Update vehicle
        /// </summary>
        /// <param name="id"></param>
        /// <param name="vehicle"></param>
        /// <param name="formFiles"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVehicle(string id, string number, string volume, string carBrand, string carType, IFormFile[] formFiles)
        {
            var vehicle = _context.Vehicles.Find(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            _context.Entry(vehicle).State = EntityState.Modified;
            vehicle.Number = number;
            vehicle.Volume = volume;
            vehicle.CarBrandId = carBrand;
            vehicle.CarTypeId = carType;
            var driver = _context.Drivers.FirstOrDefault(d => d.VehicleId == id);
            string userId = driver.UserId;
            var uploadedFiles = _context.UploadedFiles.Where(u => u.UserId == userId).ToList();
            _context.UploadedFiles.RemoveRange(uploadedFiles);
            if (!Directory.Exists(_environment.ContentRootPath + "\\wwwroot\\Images\\"))
            {
                Directory.CreateDirectory(_environment.ContentRootPath + "\\wwwroot\\Images\\");
            }
            long dateTime;
            string fileName;
            foreach (var formFile in formFiles)
            {
                DateTime dateDate = DateTime.Now;
                dateTime = long.Parse(dateDate.Year.ToString()) * 100000000 + long.Parse(dateDate.Month.ToString()) * 1000000 + long.Parse(dateDate.Day.ToString()) * 10000 + long.Parse(dateDate.Hour.ToString()) * 100 + long.Parse(dateDate.Minute.ToString());
                fileName = dateTime.ToString() + formFile.FileName;
                using (FileStream filestream = System.IO.File.Create(_environment.ContentRootPath + "\\wwwroot\\Images\\" + fileName))
                {
                    formFile.CopyTo(filestream);
                    filestream.Flush();
                }
                UploadedFile uploadedFile = new UploadedFile
                {
                    FilePath = fileName,
                    FileType = FileType.Vehicle,
                    UserId = userId
                };
                await _context.UploadedFiles.AddAsync(uploadedFile);
            }
            try
            {
                await _context.SaveChangesAsync();
                return Ok(new Response { Msg = "Vehicle was updated successfuly", Status = "1", Data = null, Code = "200" });
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
        /// <param name="formFiles"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        [Route("Create")]
        [HttpPost]
        public async Task<IActionResult> PostVehicle(string userID,string number,string volume,string carBrand,string carType,IFormFile[] formFiles)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userID);
                if (user!=null)
                {
                    //var storedVehicle = _mapper.Map<Vehicle>(vehicle);
                    var storedVehicle = new Vehicle { CarBrandId = carBrand, CarTypeId = carType, Number = number, Volume = volume };
                    _context.Vehicles.Add(storedVehicle);
                    Driver driver = _context.Drivers.FirstOrDefault(d => d.UserId == userID);
                    driver.VehicleId = storedVehicle.Id;
                    _context.Drivers.Update(driver);
                    if (formFiles == null)
                    {
                        return Ok(new Response { Code = "200", Data = new { vehicleId = storedVehicle.Id }, Msg = "Please upload one file at least", Status = "0" });
                    }
                    if (!Directory.Exists(_environment.ContentRootPath + "\\wwwroot\\Images\\"))
                    {
                        Directory.CreateDirectory(_environment.ContentRootPath + "\\wwwroot\\Images\\");
                    }
                    long dateTime;
                    string fileName;
                    foreach (var formFile in formFiles)
                    {
                        DateTime dateDate = DateTime.Now;
                        dateTime = long.Parse(dateDate.Year.ToString()) * 100000000 + long.Parse(dateDate.Month.ToString()) * 1000000 + long.Parse(dateDate.Day.ToString()) * 10000 + long.Parse(dateDate.Hour.ToString()) * 100 + long.Parse(dateDate.Minute.ToString());
                        fileName = dateTime.ToString() + formFile.FileName;
                        using (FileStream filestream = System.IO.File.Create(_environment.ContentRootPath + "\\wwwroot\\Images\\" + fileName))
                        {
                            formFile.CopyTo(filestream);
                            filestream.Flush();
                        }
                        UploadedFile uploadedFile = new UploadedFile
                        {
                            FilePath = fileName,
                            FileType = FileType.Vehicle,
                            UserId = userID
                        };
                        await _context.UploadedFiles.AddAsync(uploadedFile);
                    }
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
                    Status = "0",
                    Data = null,
                    Code = "500"
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
