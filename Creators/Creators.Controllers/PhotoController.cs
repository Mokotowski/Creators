using Microsoft.AspNetCore.Mvc;

namespace Creators.Creators.Controllers
{
    public class PhotoController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> ManagePhotos()
        {
            //przez usera
        }

        [HttpGet]
        public async Task<IActionResult> CheckLikes()
        {

        }
        [HttpGet]
        public async Task<IActionResult> CheckComments()
        {

        }



        [HttpPost]
        public async Task<IActionResult> AddPhoto(string Description, bool CommentsOpen, IFormFile file)
        {
            //obsługa
        }

        [HttpPost]
        public async Task<IActionResult> DeletePhoto(int Id)
        {
            //obsługa
        }

        [HttpGet]
        public async Task<IActionResult> CretorPhotos(string Id_Photos) // do zarządzania swojmi zdjeciami
        {
            //obsługa
        }

        [HttpGet]
        public async Task<IActionResult> UserCreatorsPhotos() // wielu twórców
        {
            //obsługa
        }

        [HttpGet]
        public async Task<IActionResult> UserCreatorPhotos() // jednego twórców
        {
            //obsługa
        }



        [HttpPost]
        public async Task<IActionResult> LikePhoto(string HeartGroup)
        {

        }
        [HttpPost]

        public async Task<IActionResult> UnLinkePhoto(int Id)
        {

        }


        [HttpPost]
        public async Task<IActionResult> AddComment(string CommentsGroup)
        {

        }

        [HttpPost]
        public async Task<IActionResult> DeleteComment(int Id)
        {

        }

        [HttpPost]
        public async Task<IActionResult> ChangeVisibleComment(int Id)
        {

        }
    }
}
