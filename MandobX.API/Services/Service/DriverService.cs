using MandobX.API.Authentication;
using MandobX.API.Data;
using MandobX.API.Models;
using MandobX.API.Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MandobX.API.Services.Service
{
    
    public class DriverService : IDriverService
    {
        private readonly ApplicationDbContext _context;
        public DriverService(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool BlockDriver(string Id)
        {
            Driver driver = _context.Drivers.Find(Id);
            if (driver!=null)
            {
                driver.User.UserStatus = UserStatus.Blocked;
                _context.Drivers.Update(driver);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool UnBlockDriver(string Id)
        {
            Driver driver = _context.Drivers.Find(Id);
            if (driver != null)
            {
                if (driver.User.AccountActivated)
                {
                    driver.User.UserStatus = UserStatus.Active;
                }
                else
                {
                    driver.User.UserStatus = UserStatus.InActive;
                }
                _context.Drivers.Update(driver);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
