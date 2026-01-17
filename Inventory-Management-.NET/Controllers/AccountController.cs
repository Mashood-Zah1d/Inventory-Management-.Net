using BCrypt.Net;
using Inventory_Management_.NET.Data;
using Inventory_Management_.NET.Dtos;
using Inventory_Management_.NET.Models;
using Inventory_Management_.NET.Models.Entities;
using Inventory_Management_.NET.Services;
using Inventory_Management_.NET.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
   
        private readonly EmailService emailService;
        private readonly AccountService accountService;

        public AccountController(EmailService emailService,AccountService accountService)
        {
            
           
            this.emailService = emailService;
            this.accountService = accountService;
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

            var result = await accountService.AddUserAsync(model);

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
            var result = await accountService.VerifyUserAsync(model);
            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                return View();
            }

            var options = new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(2)
            };

            Console.WriteLine(result.Data);
            Response.Cookies.Append("JwtToken", result.Data, options);

            return RedirectToAction("Index","Home");
        }
        private string GenerateResetCode()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> SendMail(string email)
        {
            var response = await accountService.GetUserByEmailAsync(email);
            if (!response.Success)
            {
                ModelState.AddModelError("", response.Message);
                ViewBag.Email = email;
                return View("ForgotPassword");
            }

            var code = GenerateResetCode();

            await accountService.SaveResetCodeAsync(email, code);

            var dto = new ForgotPasswordDto
            {
                Email = email,
                Code = code
            };

            await accountService.SendPasswordResetEmailAsync(dto);

            ViewBag.Email = email;
            ViewBag.CodeSent = true;
            ViewBag.SuccessMessage = "Verification code sent to your email";

            return View("ForgotPassword");
        }



        [HttpPost]
        public async Task<IActionResult> VerifyCode(string email, string userCode)
        {
            var isValid = await accountService.VerifyResetCodeAsync(email, userCode);

            if (!isValid)
            {
                ModelState.AddModelError("", "Invalid or expired code.");
                ViewBag.Email = email;
                ViewBag.CodeSent = true;
                return View("ForgotPassword");
            }

            return RedirectToAction("SetNewPassword", new { email });
        }

        [HttpGet]
        public IActionResult SetNewPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
                return RedirectToAction("ForgotPassword");

            ViewBag.Email = email;
            return View();
        }

        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> SetNewPassword(setNewPasswordViewModel model)
        {

            var result = await accountService.UpdatePasswordAsync(model);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                return View(model);
            }

            TempData["SuccessMessage"] = "Password updated successfully! You can now login.";
            return RedirectToAction("Login", "Account");
        }



    }
}
