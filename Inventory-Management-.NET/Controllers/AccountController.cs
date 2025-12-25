using BCrypt.Net;
using Inventory_Management_.NET.Data;
using Inventory_Management_.NET.Dtos;
using Inventory_Management_.NET.Models;
using Inventory_Management_.NET.Models.Entities;
using Inventory_Management_.NET.Services;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Inventory_Management_.NET.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly AccountService accountService;
        private readonly IConfiguration config;

        public AccountController (ApplicationDbContext dbContext,AccountService accountService, IConfiguration config)
        {
            this.dbContext = dbContext;
            this.accountService = accountService;
            this.config = config;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Signup()
        {
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Signup(SignupViewModel model)
        {
            var dto = new SignupDto
            {

                UserName = model.UserName,
                Name = model.Name,
                Email = model.Email,
                Password = model.Password,
                secretKey = model.secretKey
            };

            var result = await accountService.AddUserAsync(dto);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                return View(model);
            }

            TempData["SuccessMessage"] = "User Created SuccessFully";


            return RedirectToAction("Login","Account");
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var dto = new LoginDto
            {
                Password = model.Password,
                UserName = model.UserName,
            };
            var result = await accountService.VerifyUserAsync(dto);
            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
            }

            var options = new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(2)
            };

            Response.Cookies.Append("JwtToken", result.Data, options);

            return RedirectToAction("Index","Home");
        }
    }
}
