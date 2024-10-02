using Creators.Creators.Database;
using Creators.Creators.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace Creators.Creators.Services
{
    public class PhotosManagerServices : IPhotosManage
    {
        private readonly ILogger<PhotosManagerServices> _logger;
        private readonly DatabaseContext _databaseContext;

        public PhotosManagerServices(ILogger<PhotosManagerServices> logger, DatabaseContext databaseContext)
        {
            _logger = logger;
            _databaseContext = databaseContext;
        }

        public async Task AddPhoto(string Description, bool CommentsOpen, byte[] File, string FileExtension, UserModel user)
        {
            _logger.LogInformation("Starting AddPhoto for user {UserId}", user.Id);

            var creatorPage = await _databaseContext.CreatorPage
                .FirstOrDefaultAsync(p => p.Id_Creator == user.Id);
            if (creatorPage == null)
            {
                _logger.LogWarning("CreatorPage not found for user {UserId}", user.Id);
                return;
            }

            string CommentsGroup;
            string HeartGroup;

            do
            {
                CommentsGroup = Guid.NewGuid().ToString();
                HeartGroup = Guid.NewGuid().ToString();
            }
            while (await _databaseContext.CreatorPhoto
                       .AnyAsync(p => p.CommentsGroup == CommentsGroup || p.HeartGroup == HeartGroup));

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
            await _databaseContext.SaveChangesAsync();

            _logger.LogInformation("Photo added successfully for user {UserId}", user.Id);
        }

        public async Task DeletePhoto(int Id, UserModel user)
        {
            _logger.LogInformation("Starting DeletePhoto with photo Id {PhotoId} for user {UserId}", Id, user.Id);

            var photo = await _databaseContext.CreatorPhoto.FindAsync(Id);
            if (photo == null)
            {
                _logger.LogWarning("Photo with Id {PhotoId} not found", Id);
                return;
            }

            var creatorPage = await _databaseContext.CreatorPage.FindAsync(user.Id);
            if (creatorPage == null)
            {
                _logger.LogWarning("CreatorPage not found for user {UserId}", user.Id);
                return;
            }

            if (photo.Id_Photos == creatorPage.Id_Photos)
            {
                _databaseContext.CreatorPhoto.Remove(photo);
                await _databaseContext.SaveChangesAsync();
                _logger.LogInformation("Photo deleted successfully for user {UserId}", user.Id);
            }
            else
            {
                _logger.LogWarning("Photo deletion failed. User {UserId} does not own the photo {PhotoId}", user.Id, Id);
            }
        }

        public async Task ChangeVisibleComment(int Id, UserModel user)
        {
            _logger.LogInformation("Starting ChangeVisibleComment with comment Id {CommentId} for user {UserId}", Id, user.Id);

            var comment = await _databaseContext.PhotoComments.FindAsync(Id);
            if (comment == null)
            {
                _logger.LogWarning("Comment with Id {CommentId} not found", Id);
                return;
            }

            var creatorPhoto = await _databaseContext.CreatorPhoto
                .FirstOrDefaultAsync(p => p.CommentsGroup == comment.CommentsGroup);
            if (creatorPhoto == null)
            {
                _logger.LogWarning("CreatorPhoto not found for comment group {CommentsGroup}", comment.CommentsGroup);
                return;
            }

            var creatorPage = await _databaseContext.CreatorPage.FindAsync(user.Id);
            if (creatorPage == null)
            {
                _logger.LogWarning("CreatorPage not found for user {UserId}", user.Id);
                return;
            }

            if (creatorPhoto.Id_Photos == creatorPage.Id_Photos)
            {
                comment.Hidden = !comment.Hidden;
                await _databaseContext.SaveChangesAsync();
                _logger.LogInformation("Comment visibility changed successfully for user {UserId}", user.Id);
            }
            else
            {
                _logger.LogWarning("Comment visibility change failed. User {UserId} does not own the comment {CommentId}", user.Id, Id);
            }
        }
    }
}
