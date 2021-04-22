using MandobX.API.Authentication;
using MandobX.API.Data;
using MandobX.API.Models;
using MandobX.API.Services.IService;
using MandobX.API.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace MandobX.API.Services
{
    /// <summary>
    /// identity service 
    /// </summary>
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ApplicationDbContext dbContext;
        private readonly IMessageService _messageService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="roleManager"></param>
        /// <param name="dbContext"></param>
        public IdentityService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext dbContext, IMessageService messageService)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.dbContext = dbContext;
            _messageService = messageService;
        }
        /// <summary>
        /// Register for trader and driver
        /// </summary>
        /// <param name="registerModel"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task<Response> Register(RegisterModel registerModel, string role)
        {
            try
            {
                var userExist = await userManager.FindByNameAsync(registerModel.UserName);
                if (userExist != null)
                    return new Response { Status = "0", Msg = "User already Exist with the name " + registerModel.UserName };
                ApplicationUser user = new ApplicationUser
                {
                    UserName = registerModel.UserName,
                    Email = registerModel.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserStatus = UserStatus.Active,
                    UserType = role,
                    PhoneNumber = registerModel.PhoneNumber
                };
                var result = await userManager.CreateAsync(user, registerModel.Password);
                await dbContext.SaveChangesAsync();
                if(result.Succeeded)
                {
                    user = await userManager.FindByNameAsync(registerModel.UserName);
                    var verificationCode = _messageService.SendMessage(registerModel.PhoneNumber, "your + delivery + code + is +");
                    //while (true)
                    //{
                    //    if (verificationCode.IsCompleted)
                    //    {
                    //        break;
                    //    }
                    //}
                    switch (role)
                    {
                        case UserRoles.Driver:
                            user.VerificationCode = await verificationCode;
                            user.UserStatus = UserStatus.PendingMessageApproval;
                            var driver = new Driver
                            {
                                UserId = user.Id,
                                Points = 0,
                            };
                            dbContext.Drivers.Add(driver);
                            var x = await dbContext.SaveChangesAsync();
                            break;
                        case UserRoles.Trader:
                            user.VerificationCode = await verificationCode;
                            user.UserStatus = UserStatus.PendingMessageApproval;
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
                }
                if (!result.Succeeded)
                {
                    string msg = "";
                    foreach (var error in result.Errors)
                    {
                        msg = msg + error.Description + ",";
                    }
                    return new Response { Status = "0", Msg = msg };
                }
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
                if (await roleManager.RoleExistsAsync(role))
                    await userManager.AddToRoleAsync(user, role);
                return new Response { Data = new { userId = user.Id, userType = user.UserType}, Status = "1", Msg = string.Format("{0} was Created Successfully", role) };
            }

            catch (Exception e)
            {
                return new Response { Code = "500", Data = null, Msg = string.Format("{0} and internal exception is {1}", e.Message, e.InnerException == null ? "nothing" : e.InnerException.Message), Status = "0" };
            }

        }

        /// <summary>
        /// verify user code sent to its mobile
        /// </summary>
        /// <param name="verificationCode"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<Response> VerifyUser(string verificationCode, string userId)
        {
            try
            {
                var user = await userManager.FindByIdAsync(userId);
                if (user!=null)
                {
                    if (user.UserStatus == UserStatus.PendingMessageApproval)
                    {
                        if (user.VerificationCode == int.Parse(verificationCode))
                        {
                            user.UserStatus = UserStatus.Active;
                            user.PhoneNumberConfirmed = true;
                            await userManager.UpdateAsync(user);
                            return new Response { Data = null, Status = "1", Msg = "User Verified succssfuly", Code = "200" };
                        }
                        return new Response { Data = null, Status = "1", Msg = "User was not Verified succssfuly" };
                    }
                    return new Response { Data = null, Status = "1", Msg = "User is not waiting for versification code" };
                }
                else
                {
                    return new Response { Data = null, Status = "0", Msg = "User was not found", Code = "500" };
                }
            }
            catch (Exception e)
            {
                return new Response { Code = "500", Data = null, Msg = string.Format("{0} and internal exception is {1}", e.Message, e.InnerException == null ? "nothing" : e.InnerException.Message), Status = "0" };
            }
        }
    }
}
