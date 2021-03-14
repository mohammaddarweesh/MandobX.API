﻿using MandobX.API.Authentication;
using MandobX.API.Data;
using MandobX.API.Models;
using MandobX.API.Services.IService;
using MandobX.API.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace MandobX.API.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ApplicationDbContext dbContext;
        public IdentityService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext dbContext)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.dbContext = dbContext;
        }
        public async Task<Response> Register(RegisterModel registerModel, string role)
        {
            try
            {
                var userExist = await userManager.FindByNameAsync(registerModel.UserName);
                if (userExist != null)
                    return new Response { Status = "0", Msg = "User already Exist" };
                ApplicationUser user = new ApplicationUser
                {
                    UserName = registerModel.UserName,
                    Email = registerModel.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserStatus = UserStatus.InActive

                };
                var result = await userManager.CreateAsync(user, registerModel.Password);
                user = await userManager.FindByNameAsync(registerModel.UserName);
                switch (role)
                {
                    case UserRoles.Driver:
                        var driver = new Driver
                        {
                            UserId = user.Id,
                            Points = 0,
                        };
                        dbContext.Drivers.Add(driver);
                        var x = await dbContext.SaveChangesAsync();
                        break;
                    case UserRoles.Trader:
                        var trader = new Trader
                        {
                            UserId = user.Id,
                            Points = 0,
                        };
                        dbContext.Traders.Add(trader);
                        dbContext.SaveChanges();
                        break;
                    default:
                        break;
                }
                if (!result.Succeeded)
                    return new Response { Status = "0", Msg = "Something went wrong please try again later" };
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
                if (await roleManager.RoleExistsAsync(role))
                    await userManager.AddToRoleAsync(user, role);
                return new Response { Status = "1", Msg = string.Format("{0} was Created Successfully", role) };
            }

            catch (Exception e)
            {
                throw;
            }

        }
    }
}