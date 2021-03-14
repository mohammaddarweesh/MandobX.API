using AutoMapper;
using MandobX.API.Authentication;
using MandobX.API.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MandobX.API.Controllers
{
    public class MandobxBaseController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper _mapper;
        public MandobxBaseController(UserManager<ApplicationUser> userManager, IMapper mapper, ApplicationDbContext context)
        {
            this.userManager = userManager;
            _context = context;
            _mapper = mapper;
        }
        public MandobxBaseController(IMapper mapper, ApplicationDbContext context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<string> ApplicationUserId()
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
            }else
            {
                return "User Not Found";
            }
            return "";
        }
    }
}
