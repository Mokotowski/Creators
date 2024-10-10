using Creators.Creators.Database;
using Creators.Creators.Models;
using Creators.Creators.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Creators.Creators.Controllers
{
    public class PhotoController : Controller
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly ILogger<PhotoController> _logger;
        private readonly IPhotosManage _photosManage;
        private readonly ILikes _likes;
        private readonly IComments _comments;
        private readonly IPhotoDataGet _photoDataGet;
        private readonly DatabaseContext _databaseContext;

        public PhotoController(UserManager<UserModel> userManager, ILogger<PhotoController> logger, IPhotosManage photosManage, ILikes likes, IComments comments, IPhotoDataGet photoDataGet, DatabaseContext databaseContext)
        {
            _userManager = userManager;
            _logger = logger;
            _photosManage = photosManage;
            _likes = likes;
            _comments = comments;
            _photoDataGet = photoDataGet;
            _databaseContext = databaseContext;
        }

        [HttpGet]
        public async Task<IActionResult> ManagePhotos()
        {
            UserModel user = await _userManager.GetUserAsync(User);
            CreatorPage creatorPage = _databaseContext.CreatorPage.Single(p => p.Id_Creator == user.Id);
            return View(creatorPage);
        }

        [HttpGet]
        public async Task<IActionResult> CheckLikesUser(int Id)
        {
            UserModel user = await _userManager.GetUserAsync(User);
            List<HeartsForUser> photoHearts = await _photoDataGet.GetLikesUser(Id);
            return View(photoHearts);

        }
        [HttpGet]
        public async Task<IActionResult> CheckCommentsUser(int Id)
        {
            UserModel user = await _userManager.GetUserAsync(User);
            List<CommentsForUser> photoComments = await _photoDataGet.GetCommentsUser(Id, user);

            return View(photoComments);
        }


        [HttpGet]
        public async Task<IActionResult> CheckLikesCreator(int Id)
        {
            UserModel user = await _userManager.GetUserAsync(User);
            List<PhotoHearts> photoHearts = await _photoDataGet.GetLikesCreator(Id, user);

            return View(photoHearts);            
        }
        [HttpGet]
        public async Task<IActionResult> CheckCommentsCreator(int Id)
        {
            UserModel user = await _userManager.GetUserAsync(User);
            List<PhotoComments> photoComments = await _photoDataGet.GetCommentsCreator(Id, user);

            return View(photoComments);
        }




        [HttpPost]
        public async Task<IActionResult> AddPhoto(string Description, bool CommentsOpen, IFormFile file)
        {
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                byte[] fileBytes = stream.ToArray();

                await _photosManage.AddPhoto(Description, CommentsOpen, fileBytes, Path.GetExtension(file.FileName).ToLower(), await _userManager.GetUserAsync(User));
            }

            return RedirectToAction("ManagePhotos");
        }

        [HttpPost]
        public async Task<IActionResult> DeletePhoto(int Id)
        {
            await _photosManage.DeletePhoto(Id, await _userManager.GetUserAsync(User));

            return RedirectToAction("ManagePhotos");
        }

        [HttpGet]
        public async Task<IActionResult> CreatorPhotos(string Id_Photos) // do zarządzania swoimi zdjeciami jako twórcaa
        {
            HttpContext.Session.SetString("Id_Photos", Id_Photos);

            UserModel user = await _userManager.GetUserAsync(User);
            ViewBag.Id_User = user.Id; 
            List<PhotoForCreator> photoForCreator = await _photoDataGet.GetPhotosCreator(user);

            return View(photoForCreator);
        }

        [HttpGet]
        public async Task<IActionResult> UserCreatorsPhotos() // wielu twórców
        {
            UserModel user = await _userManager.GetUserAsync(User);
            ViewBag.Id_User = user.Id;
            List<PhotoForUser> photoForUser = await _photoDataGet.GetCreatorsPhotosUser(user);

            return View(photoForUser);
        }

        [HttpGet]
        public async Task<IActionResult> UserCreatorPhotos(string Id_Photos) // jednego twórców
        {
            HttpContext.Session.SetString("Id_Photos", Id_Photos);

            UserModel user = await _userManager.GetUserAsync(User);
            ViewBag.Id_User = user.Id;
            List<PhotoForUser> photoForUsers = await _photoDataGet.GetCreatorPhotosUser(Id_Photos, user);
            
            return View(photoForUsers);
        }



        [HttpPost]
        public async Task<IActionResult> LikePhoto(string HeartGroup)
        {
            UserModel user = await _userManager.GetUserAsync(User);
            await _likes.LikePhoto(HeartGroup, user.Id);

            return await GetRedirect();
        }
        [HttpPost]
        public async Task<IActionResult> UnLinkePhoto(int Id)
        {
            UserModel user = await _userManager.GetUserAsync(User);
            await _likes.UnLinkePhoto(Id, user.Id);

            return await GetRedirect();
        }


        [HttpPost]
        public async Task<IActionResult> AddComment(string CommentsGroup, string Text)
        {
            UserModel user = await _userManager.GetUserAsync(User);
            _comments.AddComment(user.Id, CommentsGroup, Text);

            return await GetRedirect();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteComment(int Id)
        {
            UserModel user = await _userManager.GetUserAsync(User);
            _comments.DeleteComment(Id, user);

            return await GetRedirect();
        }

        [HttpPost]
        public async Task<IActionResult> ChangeVisibleComment(int Id)
        {
            UserModel user = await _userManager.GetUserAsync(User);
            await _photosManage.ChangeVisibleComment(Id, user);

            return RedirectToAction("CreatorPhotos", new { Id_Photos = await _photoDataGet.GetId_Photos(user)});
        }

        private async Task<IActionResult> GetRedirect()
        {
            string Id_Photos = HttpContext.Session.GetString("Id_Photos");

            if (string.IsNullOrEmpty(Id_Photos))
            {
                return RedirectToAction("UserCreatorsPhotos");
            }
            else
            {
                return RedirectToAction("UserCreatorPhotos", new { Id_Photos = Id_Photos });
            }
        }


    }
}
