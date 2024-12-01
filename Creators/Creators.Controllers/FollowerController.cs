using Creators.Creators.Database;
using Creators.Creators.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Creators.Creators.Controllers
{
    [Authorize]
    public class FollowerController : Controller
    {
        private readonly IFollow _follow;
        private readonly IGetFollowers _getFollowers;
        private readonly UserManager<UserModel> _userManager; 
        public FollowerController(IFollow follow, IGetFollowers getFollowers, UserManager<UserModel> userManager) 
        { 
            _follow = follow;
            _getFollowers = getFollowers;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Follow(string Id_Creator)
        {
            _follow.Follow(Id_Creator, await _userManager.GetUserAsync(User));
            return RedirectToAction("ShowPage", "Creator", new { Id_Creator = Id_Creator });
        }

        [HttpGet]
        public async Task<IActionResult> UnFollow(string Id_Creator)
        {
            _follow.UnFollow(Id_Creator, await _userManager.GetUserAsync(User));
            return RedirectToAction("ShowPage", "Creator", new { Id_Creator = Id_Creator });
        }

        [HttpGet]
        public async Task<IActionResult> GetFollowersCreator(string Id_Creator)
        {
            List<Followers> followers = await _getFollowers.GetCreatorFollowers(Id_Creator);
            return View(followers);
        }

        [HttpGet]
        public async Task<IActionResult> GetFollowingCreators()
        {
            List<Followers> following = await _getFollowers.GetUserFollowing(await _userManager.GetUserAsync(User));
            return View(following);
        }



    }
}
