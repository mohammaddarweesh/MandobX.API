using MandobX.API.Authentication;
using MandobX.API.Data;
using MandobX.API.Models;
using MandobX.API.Services.IService;
using MandobX.API.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MandobX.API.Controllers
{
    /// <summary>
    /// Authentaication controler
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;
        private readonly IIdentityService identityService;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="roleManager"></param>
        /// <param name="configuration"></param>
        /// <param name="identityService"></param>
        /// <param name="signInManager"></param>
        /// <param name="context"></param>
        /// <param name="environment"></param>
        public AuthenticationController(UserManager<ApplicationUser> userManager,
                                        RoleManager<IdentityRole> roleManager,
                                        IConfiguration configuration,
                                        IIdentityService identityService,
                                        SignInManager<ApplicationUser> signInManager,
                                        ApplicationDbContext context,
                                        IWebHostEnvironment environment)
        {
            _configuration = configuration;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.identityService = identityService;
            this.signInManager = signInManager;
            _context = context;
            _environment = environment;
        }
        /// <summary>
        /// login
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (string.IsNullOrEmpty(loginModel.PhoneNumber) && string.IsNullOrEmpty(loginModel.UserName))
            {
                return Ok(new Response { Code = "500", Data = null, Msg = "Please fill one of the following: user name or phone number", Status = "0" });
            }
            var user = await userManager.FindByNameAsync(loginModel.UserName);
            if (user == null && !string.IsNullOrEmpty(loginModel.PhoneNumber))
            {
                user = _context.ApplicationUsers.FirstOrDefault(a => a.PhoneNumber == loginModel.PhoneNumber);
            }
            if (user != null && await userManager.CheckPasswordAsync(user, loginModel.Password))
            {
                var userRoles = await userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, loginModel.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var token = new JwtSecurityToken(
                        issuer: _configuration["Jwt:Issuer"],
                        audience: _configuration["Jwt:Audience"],
                        expires: DateTime.Now.AddHours(3),
                        claims: authClaims,
                        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );
                await signInManager.SignInAsync(user, true);
                var response = new Response
                {
                    Status = "1",
                    Msg = "User Logged in Successfuly",
                    Data = new
                    {
                        userId = user.Id,
                        userType = user.UserType
                    }
                };

                return Ok(
                        new
                        {
                            response = response,
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo,
                        });
            }
            return Unauthorized();
        }

        /// <summary>
        /// Log out
        /// </summary>
        /// <returns></returns>

        [Route("logout")]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return Ok(new Response { Code = "200", Data = null, Msg = "User Loged Out Successfuly", Status = "1" });

        }
        /// <summary>
        /// Register Driver
        /// </summary>
        /// <param name="registerModel"></param>
        /// <returns></returns>
        [Route("registerdriver")]
        [HttpPost]
        public async Task<IActionResult> RegisterDriver(RegisterModel registerModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Response response = await identityService.Register(registerModel, UserRoles.Driver);
                    if (response.Status == "1")
                    {
                        List<Claim> authClaims = new List<Claim> {
                        new Claim(ClaimTypes.Role, UserRoles.Driver),
                        new Claim(ClaimTypes.Name, registerModel.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    };
                        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                        var token = new JwtSecurityToken(
                                issuer: _configuration["Jwt:Issuer"],
                                audience: _configuration["Jwt:Audience"],
                                expires: DateTime.Now.AddHours(3),
                                claims: authClaims,
                                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                            );
                        return Ok(new { response = response, token = new JwtSecurityTokenHandler().WriteToken(token), expiration = token.ValidTo });
                    }
                    return Ok(response);
                }
                catch (Exception e)
                {

                    return StatusCode(StatusCodes.Status417ExpectationFailed, new Response { Msg = e.Message, Status = "0" });
                }
            }
            else
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0)
                           .ToList();
                string msg = "";
                foreach (var error in errors)
                {
                    msg = msg + error.ToString() + ",";
                }
                return StatusCode(StatusCodes.Status417ExpectationFailed, new Response { Msg = msg, Status = "0" });
            }
        }
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="registerModel"></param>
        ///// <returns></returns>
        //[Route("registeradmin")]
        //[HttpPost]
        //public async Task<IActionResult> RegisterAdmin(RegisterModel registerModel)
        //{
        //    try
        //    {
        //        Response response = await identityService.Register(registerModel, UserRoles.Admin);
        //        return Ok(response);
        //    }
        //    catch (Exception e)
        //    {

        //        return StatusCode(StatusCodes.Status417ExpectationFailed, new Response { Msg = e.Message, Status = "0" });
        //    }

        //}

        /// <summary>
        /// Register Trader
        /// </summary>
        /// <param name="registerModel"></param>
        /// <returns></returns>
        [Route("registertrader")]
        [HttpPost]
        public async Task<IActionResult> RegisterTrader(RegisterModel registerModel)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    Response response = await identityService.Register(registerModel, UserRoles.Trader);
                    if (response.Status == "1")
                    {
                        List<Claim> authClaims = new List<Claim> {
                        new Claim(ClaimTypes.Role, UserRoles.Trader),
                        new Claim(ClaimTypes.Name, registerModel.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    };
                        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                        var token = new JwtSecurityToken(
                                issuer: _configuration["Jwt:Issuer"],
                                audience: _configuration["Jwt:Audience"],
                                expires: DateTime.Now.AddHours(3),
                                claims: authClaims,
                                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                            );
                        return Ok(new { response = response, token = new JwtSecurityTokenHandler().WriteToken(token), expiration = token.ValidTo });
                    }
                    return Ok(response);
                }
                else
                {
                    var errors = ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0)
                           .ToList();
                    string msg = "";
                    foreach (var error in errors)
                    {
                        msg = msg + error.ToString() + ",";
                    }
                    return StatusCode(StatusCodes.Status417ExpectationFailed, new Response { Msg = msg, Status = "0" });
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status417ExpectationFailed, new Response { Msg = e.Message, Status = "0" });
            }
        }
        /// <summary>
        /// upload images to driver or trader
        /// </summary>
        /// <param name="formFiles"></param>
        /// <param name="userId"></param>
        /// <param name="fileType"></param>
        /// <returns></returns>
        [Route("uploadimages")]
        [HttpPost]
        public async Task<IActionResult> UploadImages(IFormFile[] formFiles, string userId, string fileType)
        {
            try
            {
                if (formFiles == null)
                {
                    return Ok(new Response { Code = "200", Data = null, Msg = "Please upload one file at least", Status = "0" });
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
                        FileType = (FileType)int.Parse(fileType),
                        UserId = userId
                    };
                    await _context.UploadedFiles.AddAsync(uploadedFile);
                }
                await _context.SaveChangesAsync();
                return Ok(new Response { Code = "200", Data = null, Msg = "Files Uploaded Successfuly", Status = "1" });
            }
            catch (Exception e)
            {
                return BadRequest(new Response { Code = "500", Data = null, Msg = string.Format("{0} and internal exception is {1}", e.Message, e.InnerException == null ? "nothing" : e.InnerException.Message), Status = "0" });
            }
        }

        /// <summary>
        /// Verify user account
        /// </summary>
        /// <param name="verificationCode"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost("verifyaccount")]
        public async Task<IActionResult> VerifyAccount(string verificationCode, string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                if (!string.IsNullOrEmpty(verificationCode))
                {
                    var res = await identityService.VerifyUser(verificationCode, userId);
                    return Ok(new Response { Code = "200", Data = null, Msg = "Account Verified Successfuly", Status = "1" });
                }
                else
                {
                    return Ok(new Response { Code = "200", Data = null, Msg = "please add the versification code", Status = "0" });
                }
            }
            else
            {
                return NotFound(new Response { Code = "200", Data = null, Msg = "User not found", Status = "0" });
            }
        }
    }
}
