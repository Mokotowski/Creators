using Creators.Creators.Database;
using Creators.Creators.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Creators.Creators.Controllers
{
    [Authorize]
    public class AnnouncementController : Controller
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly IAnnouncementData _announcementData;
        private readonly IAnnouncementManage _announcementManage;
        public AnnouncementController(UserManager<UserModel> userManager, IAnnouncementData announcementData, IAnnouncementManage announcementManage) 
        { 
            _userManager = userManager;
            _announcementData = announcementData;
            _announcementManage = announcementManage;
        }

        [HttpGet]
        public async Task<IActionResult> ManageAnnouncement()
        {
            UserModel user = await _userManager.GetUserAsync(User);
            List<CreatorAnnouncement> announcements = await _announcementManage.MyAnnouncement(user);

            return View(announcements);
        }

        [HttpPost]
        public async Task<IActionResult> AddAnnouncement(string Title, string Description)
        {
            UserModel user = await _userManager.GetUserAsync(User);
            await _announcementManage.AddAnnouncement(user, Title, Description);

            return RedirectToAction("ManageAnnouncement", "Announcement");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAnnouncement(int Id)
        {
            UserModel user = await _userManager.GetUserAsync(User);
            await _announcementManage.DeleteAnnouncement(user, Id);

            return RedirectToAction("ManageAnnouncement", "Announcement");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAnnouncement(int Id, string Title, string Description)
        {
            UserModel user = await _userManager.GetUserAsync(User);
            await _announcementManage.UpdateAnnouncement(user, Id, Title, Description);

            return RedirectToAction("ManageAnnouncement", "Announcement");
        }

        [HttpGet]
        public async Task<IActionResult> CreatorAnnouncement(string Id_Announcement)
        {
            List<CreatorAnnouncement> announcements = await _announcementData.CreatorAnnouncements(Id_Announcement);

            return View(announcements);
        }

        [HttpGet]
        public async Task<IActionResult> MyCretorsAnnouncement()
        {
            UserModel user = await _userManager.GetUserAsync(User);
            List<CreatorAnnouncement> announcements = await _announcementData.MyCreatorsAnnouncement(user);

            return View(announcements);
        }




    }
}