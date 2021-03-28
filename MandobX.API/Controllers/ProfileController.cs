using AutoMapper;
using MandobX.API.Authentication;
using MandobX.API.Data;
using MandobX.API.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
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
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetDriverOrTrader(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                
                if (user.UserType == UserRoles.Driver)
                {
                    var driver = _context.Drivers.FirstOrDefault(d => d.UserId == userId);
                    EditProfileViewModel details = _mapper.Map<EditProfileViewModel>(driver);
                    if (driver != null)
                    {
                        return Ok(new Response { Code = "200", Data = new { CurrentUser = details, UserType = UserRoles.Driver }, Msg = "", Status = "1" });
                    }
                }
                else if(user.UserType == UserRoles.Trader)
                {
                    var trader = _context.Traders.FirstOrDefault(d => d.UserId == userId);
                    if (trader != null)
                    {
                        return Ok(new Response { Code = "200", Data = new { CurrentUser = trader, UserType = UserRoles.Trader }, Msg = "", Status = "1" });
                    }
                }
            }
            return NotFound(new Response { Msg = "User Not Found"});
        }

        /// <summary>
        /// Update user details driver or trader
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UpdateUser(EditProfileViewModel model)
        {
            if (model!=null)
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                if (user.UserType == UserRoles.Driver)
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
                else if(user.UserType == UserRoles.Trader)
                {
                    var trader = _context.Traders.FirstOrDefault(d => d.UserId == model.Id);
                    trader.FirstName = model.FirstName;
                    trader.LastName = model.LastName;
                    trader.User.Email = model.EmailAddress;
                    trader.User.PhoneNumber = model.PhoneNumber;
                    _context.Traders.Update(trader);
                }
                await _context.SaveChangesAsync();
                return Ok(new Response { Code = "200", Data = null, Msg = "User Updated successfuly", Status = "1" });
            }
            return BadRequest();
        }

    }
}
