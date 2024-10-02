using Creators.Creators.Database;
using Creators.Creators.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Creators.Creators.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly IEventsFunctions _eventsFunctions;
        private readonly UserManager<UserModel> _userManager;
        private readonly IScheduleData _scheduleData;
        public ScheduleController(IEventsFunctions eventsFunctions, UserManager<UserModel> userManager, IScheduleData scheduleData) 
        { 
            _eventsFunctions = eventsFunctions;
            _userManager = userManager;
            _scheduleData = scheduleData;
        }
        [HttpGet]
        public async Task<IActionResult> ShowSchedule(string Id_Calendar)
        {
            List<CalendarEvents> events = await _eventsFunctions.ShowSchedule(Id_Calendar);
            UserModel user = await _userManager.GetUserAsync(User);
            ViewBag.IsCreator = await _scheduleData.IsCreator(Id_Calendar, user);
            ViewBag.User = user;
            return View(events);
        }

        [HttpPost]
        public async Task<IActionResult> AddEvent(DateOnly date, TimeSpan start, TimeSpan end, string description)
        {
            UserModel user = await _userManager.GetUserAsync(User);
            string result = await _eventsFunctions.AddEvent(user, date, start, end, description);

            if (result == "success")
            {
                return RedirectToAction("ShowSchedule", "Schedule", new { Id_Calendar = user.Id });
            }
            else
            {
                return RedirectToAction("ErrorEventFunction", "Schedule");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteEvent(int Id)
        {
            UserModel user = await _userManager.GetUserAsync(User);
            string result = await _eventsFunctions.DeleteEvent(user, Id);

            if (result == "success")
            {
                return RedirectToAction("ShowSchedule", "Schedule", new { Id_Calendar = user.Id });
            }
            else
            {
                return RedirectToAction("ErrorEventFunction", "Schedule");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateEvent(int Id, DateOnly date, TimeSpan start, TimeSpan end, string description)
        {
            UserModel user = await _userManager.GetUserAsync(User);
            string result = await _eventsFunctions.UpdateEvent(user, Id, date, start, end, description);

            if (result == "success")
            {
                return RedirectToAction("ShowSchedule", "Schedule", new { Id_Calendar = user.Id });
            }
            else
            {
                return RedirectToAction("ErrorEventFunction", "Schedule");
            }
        }



        [HttpGet]
        public async Task<IActionResult> ScheduleManage(string Id_Calendar)
        {
            UserModel user = await _userManager.GetUserAsync(User);

            if (user.Id != Id_Calendar)
            {
                return RedirectToAction("ErrorEventFunction", "Schedule");
            }

            var events = await _eventsFunctions.ShowSchedule(Id_Calendar);
            return View(events);
        }

        [HttpGet]
        public IActionResult ErrorEventFunction()
        {
            return View();
        }
    }
}
