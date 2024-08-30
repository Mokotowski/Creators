﻿using Creators.Creators.Database;
using Creators.Creators.Models;
using Creators.Creators.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Creators.Creators.Controllers
{
    public class CreatorController : Controller
    {
        private readonly IPageFunctions _pageFunctions;
        private readonly UserManager<UserModel> _userManager;
        private readonly SignInManager<UserModel> _signInManager;
        private readonly ISendEmail _sendEmail;
        private readonly IFunctionsFromEmail _functionsFromEmail;
        private readonly IFollow _follow;
        public CreatorController(IPageFunctions pageFunctions, UserManager<UserModel> userManager, SignInManager<UserModel> signInManager, ISendEmail sendEmail, IFunctionsFromEmail functionsFromEmail, IFollow follow) 
        { 
            _pageFunctions = pageFunctions;
            _userManager = userManager;
            _signInManager = signInManager;
            _sendEmail = sendEmail;
            _functionsFromEmail = functionsFromEmail;
            _follow = follow;
        }

        [HttpGet]
        public IActionResult BecomeCreator()
        {
            return View();
        }

        [HttpGet]
        public IActionResult BecomeCreatorRegisterPlatform()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> BecomeCreatorRegisterPlatform(string AccountNumber, string Description, string ProfileImage, bool NotifyImages, bool NotifyEvents, string BioLinks)
        {
            var user = await _userManager.GetUserAsync(User);
            string result = await _pageFunctions.CreatePage(user, AccountNumber, Description, ProfileImage, NotifyImages, NotifyEvents, BioLinks);

            IActionResult actionResult;

            switch (result)
            {
                case "Exist":
                    actionResult = RedirectToAction("PageExist", new { Id_Creator = user.Id });
                    break;
                case "Error":
                    actionResult = RedirectToAction("ErrorCreatePage");
                    break;
                default:
                    actionResult = RedirectToAction("Thanks", "Creator", new { Id_Creator = user.Id });
                    break;
            } 

            return actionResult;
        }

        [HttpGet]
        public IActionResult Thanks(string Id_Creator) 
        {
            ViewBag.Id_Creator = Id_Creator;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ErrorCreatePage()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> PageExist(string Id_Creator)
        {
            ViewBag.Id_Creator = Id_Creator;
            return View();
        }






        [HttpGet]
        public async Task<IActionResult> ShowPage(string Id_Creator)
        {
            CreatorPageShow pageShow = await _pageFunctions.GetPageForUsers(Id_Creator);
            ViewBag.IsFollowing = await _follow.IsFollowing(Id_Creator, await _userManager.GetUserAsync(User));
            return View(pageShow);
        }


        [HttpGet]
        public async Task<IActionResult> UpdatePage(string Id_Creator)
        {
            CreatorPageShow pageShow = await _pageFunctions.GetPageForUpdate(Id_Creator);
            return View(pageShow);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePageRequest(string AccountNumber, string Description, string ProfileImage, bool NotifyImages, bool NotifyEvents, string BioLinks)
        {
            UserModel user = await _userManager.GetUserAsync(User);
            await _sendEmail.UpdatePage(user, AccountNumber, Description, ProfileImage, NotifyImages, NotifyEvents, BioLinks);
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> UpdatePageAction(string UserFromEmailId, string AccountNumber, string Description, string ProfileImage, bool NotifyImages, bool NotifyEvents, string BioLinks)
        {
            UserModel user = await _userManager.GetUserAsync(User);
            string result = await _functionsFromEmail.UpdatePageFinalizator(UserFromEmailId, AccountNumber, Description, ProfileImage, NotifyImages, NotifyEvents, BioLinks, user);

            return RedirectToAction("UpdatePageResult", "Creator", new { Id_Creator = user.Id, result = result });
        }

        [HttpGet]
        public async Task<IActionResult> UpdatePageResult(string Id_Creator, string result)
        {
            ViewBag.Id_Creator = Id_Creator;
            ViewBag.Result = result;
            return View();   
        }

        [HttpGet]
        public async Task<IActionResult> ManageCreatorPage()
        {
            UserModel user = await _userManager.GetUserAsync(User);
            if(user.IsCreator)
            {
                ViewBag.Id_Creator = user.Id;
                return View();
            }
            else
            {
                return RedirectToAction("NotCreator", "Creator");
            }
        }
        [HttpGet]
        public async Task<IActionResult> NotCreator()
        {
            return View();
        }





        [HttpGet]
        public async Task<IActionResult> SearchCreator()
        {
            return View();
        }





        [HttpGet]
        public async Task<IActionResult> SearchCreatorResult(string ProfileName)
        {
            List<CreatorsSearch> creators = await _pageFunctions.FindCreators(ProfileName);

            return View(creators);
        }

        

    }
}
