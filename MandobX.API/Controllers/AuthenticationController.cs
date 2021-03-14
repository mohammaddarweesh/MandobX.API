using MandobX.API.Authentication;
using MandobX.API.Services.IService;
using MandobX.API.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MandobX.API.Controllers
{
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
        public AuthenticationController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IIdentityService identityService, SignInManager<ApplicationUser> signInManager)
        {
            _configuration = configuration;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.identityService = identityService;
            this.signInManager = signInManager;
        }

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            var user = await userManager.FindByNameAsync(loginModel.UserName);
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
                await signInManager.SignInAsync(user, false);
                var response = new Response
                {
                    Status = "1",
                    Msg = "User Logged in Successfuly"
                };

                return Ok(
                        new
                        {
                            response = response,
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        });
            }
            return Unauthorized();
        }

        //logout
        [Route("logout")]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return Ok(new Response { Code = "200", Data = null, Msg = "User Loged Out Successfuly", Status = "1" });

        }

        [Route("registerdriver")]
        [HttpPost]
        public async Task<IActionResult> RegisterDriver(RegisterModel registerModel)
        {
            try
            {
                Response response = await identityService.Register(registerModel, UserRoles.Driver);
                return Ok(response);
            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status417ExpectationFailed, new Response { Msg = e.Message, Status = "0" });
            }

        }

        [Route("registeradmin")]
        [HttpPost]
        public async Task<IActionResult> RegisterAdmin(RegisterModel registerModel)
        {
            try
            {
                Response response = await identityService.Register(registerModel, UserRoles.Admin);
                return Ok(response);
            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status417ExpectationFailed, new Response { Msg = e.Message, Status = "0" });
            }

        }

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
        //        return StatusCode(StatusCodes.Status417ExpectationFailed, new Response { Message = e.Message, Status = Result.Error });
        //    }
        //}

        [Route("registertrader")]
        [HttpPost]
        public async Task<IActionResult> RegisterTrader(RegisterModel registerModel)
        {

            try
            {
                Response response = await identityService.Register(registerModel, UserRoles.Trader);
                return Ok(response);
            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status417ExpectationFailed, new Response { Msg = e.Message, Status = "0" });
            }
        }
    }
}
