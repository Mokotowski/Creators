using Creators.Creators.Database;
using Creators.Creators.Models;
using Creators.Creators.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security;

namespace Creators.Creators.Controllers
{
    public class ChatController : Controller
    {
        private readonly IBlock _block;
        private readonly IChatsActions _actions;
        private readonly IUserData _userData;
        private UserManager<UserModel> _userManager;
        public ChatController(IBlock block, IChatsActions actions, IUserData userData, UserManager<UserModel> userManager)
        {
            _block = block;
            _actions = actions;
            _userData = userData;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult SearchUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SearchUserResult(string userphrase)
        {
            List<UserForChats> users = await _userData.GetUsers(userphrase, await _userManager.GetUserAsync(User));

            return View(users);
        }




        [HttpGet]
        public async Task<IActionResult> MyChats()
        {
            UserModel user = await _userManager.GetUserAsync(User);
            List<Chats> chats = await _actions.GetChats(user);
            ViewBag.Id_User = user.Id;
            ViewBag.UserName = user.UserName;
            return View(chats);
        }

        [HttpGet]
        public async Task<IActionResult> Chat(int Id)
        {
            Chats chat = await _actions.GetChat(Id);
            UserModel user = await _userManager.GetUserAsync(User);
            ViewBag.User = user;
            return View(chat);
        }


        [HttpPost]
        public async Task<IActionResult> RefreshChat(int Id)
        {
            return RedirectToAction("Chat", new { Id = Id });
        }



        [HttpPost]
        public async Task<IActionResult> SendMessage(int Id, string Text)
        {
            await _actions.AddMessage(Id, await _userManager.GetUserAsync(User), Text);

            return RedirectToAction("Chat", new { Id = Id });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMessage(int Id)
        {
            Chats chat = await _actions.DeleteMessage(Id, await _userManager.GetUserAsync(User));   

            return RedirectToAction("Chat", new { Id = chat.Id });
        }




        [HttpPost]
        public async Task<IActionResult> MakeChat(string Id_User)
        {
            Chats chat = await _actions.MakeChat(await _userManager.GetUserAsync(User), Id_User);

            return RedirectToAction("Chat", new { Id = chat.Id });
        }




        [HttpGet]
        public async Task<IActionResult> MyBlocklist()
        {
            List<Blocklist> blocklist = await _block.GetBlockUsers(await _userManager.GetUserAsync(User));

            return View(blocklist);
        }

        [HttpPost]
        public async Task<IActionResult> BlockUser(string Id_User)
        {
            await _block.BlockUser(Id_User, await _userManager.GetUserAsync(User));

            return RedirectToAction("MyBlocklist");
        }


        [HttpPost]
        public async Task<IActionResult> UnblockUser(int Id)
        {
            await _block.UnblockUser(Id, await _userManager.GetUserAsync(User));

            return RedirectToAction("MyBlocklist");
        }



    }
}
