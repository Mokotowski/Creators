using Creators.Creators.Database;
using Creators.Creators.Services.Interface;

namespace Creators.Creators.Services
{
    public class PhotosManagerServices  : IPhotosManage
    {
        private readonly ILogger<PhotosManagerServices> _logger;
        private readonly DatabaseContext _databaseContext;
        public PhotosManagerServices(ILogger<PhotosManagerServices> logger, DatabaseContext databaseContext) 
        { 
            _logger = logger;
            _databaseContext = databaseContext;
        }

        public async Task AddPhoto(string Description, bool CommentsOpen, string File, string FileExtension, UserModel user)
        {
            CreatorPage creatorPage = _databaseContext.CreatorPage.Find(user.Id);
            string CommentsGroup = Guid.NewGuid().ToString();
            string HeartGroup = Guid.NewGuid().ToString();

            while (0 == _databaseContext.CreatorPhoto.Count(p => p.CommentsGroup == CommentsGroup) && 0 == _databaseContext.CreatorPhoto.Count(p => p.HeartGroup == HeartGroup)) 
            {
                CommentsGroup = Guid.NewGuid().ToString();
                HeartGroup = Guid.NewGuid().ToString();
            }


            CreatorPhoto photo = new CreatorPhoto()
            {
                Id_Photos = creatorPage.Id_Photos,
                CommentsGroup = CommentsGroup,
                HeartGroup = HeartGroup,
                Description = Description,
                CommentsOpen = CommentsOpen,
                File = File,
                FileExtension = FileExtension,
                DateTime = DateTime.Now,

                CreatorPage = creatorPage,
            };

            _databaseContext.CreatorPhoto.Add(photo);
            _databaseContext.SaveChanges();
        }

        public async Task DeletePhoto(int Id, UserModel user)
        {
            CreatorPhoto photo = _databaseContext.CreatorPhoto.Find(Id);
            CreatorPage creatorPage = _databaseContext.CreatorPage.Find(user.Id);

            if (photo.Id_Photos == creatorPage.Id_Photos)
            {
                _databaseContext.CreatorPhoto.Remove(photo);
                _databaseContext.SaveChanges();
            }
        }


        public async Task ChangeVisibleComment(int Id, UserModel user)
        {
            PhotoComments comment = _databaseContext.PhotoComments.Find(Id);
            CreatorPhoto creatorPhoto = _databaseContext.CreatorPhoto.Single(p => p.CommentsGroup == comment.CommentsGroup);
            CreatorPage creatorPage = _databaseContext.CreatorPage.Find(user.Id);

            if (creatorPhoto.Id_Photos == creatorPage.Id_Photos)
            {
                comment.Hidden = !comment.Hidden;
                _databaseContext.SaveChanges();
            }

        }
    }
}
