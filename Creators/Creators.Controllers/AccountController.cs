﻿using Creators.Creators.Database;
using Creators.Creators.Services;
using Creators.Creators.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Drawing.Drawing2D;

namespace Creators.Controllers
{
    public class AccountController : Controller
    {
        private readonly IRegister _register;
        private readonly ILogin _login;
        private readonly ISendEmail _sendEmail;
        private readonly IFunctionsFromEmail _functionsEmail;
        private readonly ILogout _logout;
        private readonly UserManager<UserModel> _userManager;
        private readonly SignInManager<UserModel> _signInManager;
        public AccountController(IRegister register, ILogin login, ISendEmail sendEmail, IFunctionsFromEmail functionsEmail, ILogout logout, UserManager<UserModel> userManager, SignInManager<UserModel> signInManager)
        {
            _register = register;
            _login = login;
            _sendEmail = sendEmail;
            _functionsEmail = functionsEmail;
            _logout = logout;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register() 
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SimpleRegister(string Nick, string FirstName, string LastName, string PhoneNumber, string Email, string Password)
        {
            if (await _register.SimpleRegister(Nick, FirstName, LastName, PhoneNumber, Email, Password))
            {
                await _sendEmail.SendConfirmedEmail(Email);
                return RedirectToAction("PleaseCheckEmail", new { Email = Email });
            }
            return RedirectToAction("ErrorRegister");
        }
        [HttpPost]
        public async Task ExternalRegister(string type) 
        {
            await _register.ExternalRegister(type);
        }



        [HttpGet]
        public IActionResult ErrorRegister()
        {
            return View();
        }







        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SimpleLogin(string Nick, string Password)
        {
            if(await _login.SimpleLogin(Nick, Password))
            {
                return RedirectToAction("SuccessfulLogin");
            }
            else
            {
                return RedirectToAction("ErrorLogin");
            }
        }
        [HttpPost]
        public async Task ExternalLogin()
        {
            await _login.ExternalLogin();
        }


        [HttpGet]
        public IActionResult SuccessfulLogin()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ErrorLogin()
        {
            return View();
        }




        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string Email)
        {
            await _sendEmail.SendResetPasswordEmail(Email);
            return RedirectToAction("ForgotPasswordInform", new { Email = Email });

        }
        [HttpGet]
        public IActionResult ForgotPasswordInform(string Email)
        {
            ViewBag.Email = Email;  
            return View();
        }




        [HttpGet]
        public IActionResult ResetPassword(string code, string Email)
        {
            ViewBag.Email = Email;
            ViewBag.Code = code;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(string code, string Email, string Password)
        {
            await _functionsEmail.ResetPassword(code, Email, Password);
            return RedirectToAction("Login");
        }





        [HttpPost]
        public async Task<IActionResult> Logout() 
        {
            await _logout.Logout();
            return RedirectToAction("LogoutInfo");
        }
        [HttpGet]
        public IActionResult LogoutInfo()
        {
            return View();
        }


        [HttpGet]
        public IActionResult PleaseCheckEmail(string Email)
        {
            ViewBag.Email = Email;
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string code, string Email) 
        {
            ViewBag.Code = code;
            ViewBag.Email = Email;  
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ConfirmedEmail(string code, string Email)
        {
            await _functionsEmail.ConfirmedEmail(code, Email);
            return RedirectToAction("ConfirmedEmail");
        }
        [HttpGet]
        public async Task<IActionResult> ConfirmedEmail()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ResendConfirmationEmail(string Email)
        {
            await _sendEmail.SendConfirmedEmail(Email);
            ViewBag.Email = Email;
            return View();
        }


    }
}
