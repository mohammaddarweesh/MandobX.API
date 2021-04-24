using AutoMapper;
using MandobX.API.Authentication;
using MandobX.API.Data;
using MandobX.API.Models;
using MandobX.API.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MandobX.API.Controllers
{
    /// <summary>
    /// Trader and Driver Profile Controller 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userManager"></param>
        /// <param name="mapper"></param>
        public ProfileController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }

        /// <summary>
        /// get driver or trader details to update
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("GetDriverProfile/{userId}")]
        public async Task<IActionResult> GetDriverProfile(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var driver = _context.Drivers.FirstOrDefault(d => d.UserId == userId);
                EditDriverProfileViewModel details = _mapper.Map<EditDriverProfileViewModel>(driver);
                List<UploadedFile> uploadedFiles = _context.UploadedFiles.Where(u => u.UserId == userId && u.FileType != FileType.Vehicle).ToList();
                foreach (var uploadedFile in uploadedFiles)
                {
                    uploadedFile.FilePath = "http://mori23-001-site1.dtempurl.com/images/" + uploadedFile.FilePath;
                }
                if (driver != null)
                {
                    return Ok(new Response { Code = "200", Data = new { CurrentUser = details, UserType = UserRoles.Driver, UploadedFiles = uploadedFiles }, Msg = "", Status = "1" });
                }
            }
            return NotFound(new Response { Msg = "User Not Found" });
        }

        /// <summary>
        /// get driver or trader details to update
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("GetTraderProfile/{userId}")]
        public async Task<IActionResult> GetTraderProfile(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                Trader traderContext = _context.Traders.FirstOrDefault(d => d.UserId == userId);
                EditTraderProfileViewModel trader = _mapper.Map<EditTraderProfileViewModel>(traderContext);
                List<TypeOfTrading> typeOfTradings = _context.TypeOftradings.ToList();
                List<UploadedFile> uploadedFiles = _context.UploadedFiles.Where(u => u.UserId == userId).ToList();
                foreach (var uploadedFile in uploadedFiles)
                {
                    uploadedFile.FilePath = "http://mori23-001-site1.dtempurl.com/images/" + uploadedFile.FilePath;
                }
                if (trader != null)
                {
                    return Ok(new Response { Code = "200", Data = new { CurrentUser = trader, UserType = UserRoles.Trader, TypeOfTradings = typeOfTradings, UploadedFiles = uploadedFiles }, Msg = "", Status = "1" });
                }
            }
            return NotFound(new Response { Msg = "User Not Found" });
        }

        /// <summary>
        /// Update user details driver or trader
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("UpdateDriver")]
        public async Task<IActionResult> UpdateDriver(EditDriverProfileViewModel model)
        {
            if (model != null)
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    var driver = _context.Drivers.FirstOrDefault(d => d.UserId == model.Id);
                    driver.FirstName = model.FirstName;
                    driver.LastName = model.LastName;
                    driver.User.Email = model.EmailAddress;
                    driver.User.PhoneNumber = model.PhoneNumber;
                    driver.Latitude = model.Latitude;
                    driver.Longitude = model.Longitude;
                    _context.Drivers.Update(driver);
                }
                else
                {
                    return NotFound();
                }
                await _context.SaveChangesAsync();
                return Ok(new Response { Code = "200", Data = null, Msg = "User Updated successfuly", Status = "1" });
            }
            return BadRequest();
        }

        /// <summary>
        /// Update user details driver or trader
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("UpdateTrader")]
        public async Task<IActionResult> UpdateTrader(EditTraderProfileViewModel model)
        {
            if (model != null)
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    var trader = _context.Traders.FirstOrDefault(d => d.UserId == model.Id);
                    trader.FirstName = model.FirstName;
                    trader.LastName = model.LastName;
                    trader.User.Email = model.EmailAddress;
                    trader.User.PhoneNumber = model.PhoneNumber;
                    trader.TypeOftradingId = model.TypeOftradingId;
                    _context.Traders.Update(trader);
                    await _context.SaveChangesAsync();
                    return Ok(new Response { Code = "200", Data = null, Msg = "User Updated successfuly", Status = "1" });
                }
                else
                {
                    return NotFound();
                }
            }
            return BadRequest();
        }

    }
}
