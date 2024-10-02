using Creators.Creators.Database;
using Creators.Creators.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace Creators.Creators.Services
{
    public class PhotoActionsServices : ILikes, IComments
    {
        private readonly ILogger<PhotoActionsServices> _logger;
        private readonly DatabaseContext _databaseContext;

        public PhotoActionsServices(ILogger<PhotoActionsServices> logger, DatabaseContext databaseContext)
        {
            _logger = logger;
            _databaseContext = databaseContext;
        }

        public async Task LikePhoto(string HeartGroup, string Id_User)
        {
            try
            {
                _logger.LogInformation("Attempting to like photo with HeartGroup: {HeartGroup} by user {UserId}", HeartGroup, Id_User);

                var user = await _databaseContext.Users.FindAsync(Id_User);
                var photo = await _databaseContext.CreatorPhoto.SingleOrDefaultAsync(p => p.HeartGroup == HeartGroup);

                if (user == null || photo == null)
                {
                    _logger.LogWarning("User or photo not found for like operation. User: {UserId}, HeartGroup: {HeartGroup}", Id_User, HeartGroup);
                    return;
                }

                var photoHeart = new PhotoHearts
                {
                    Id_User = Id_User,
                    User = user.UserName,
                    HeartGroup = HeartGroup,
                    CreatorPhoto = photo
                };

                _databaseContext.PhotoHearts.Add(photoHeart);
                await _databaseContext.SaveChangesAsync();
                _logger.LogInformation("Photo liked successfully by user {UserId}", Id_User);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while liking photo with HeartGroup: {HeartGroup} by user {UserId}", HeartGroup, Id_User);
            }
        }

        public async Task UnLinkePhoto(int Id, string Id_User)
        {
            try
            {
                _logger.LogInformation("Attempting to unlike photo with Id: {PhotoId} by user {UserId}", Id, Id_User);

                var photo = await _databaseContext.CreatorPhoto.FindAsync(Id);
                if (photo == null)
                {
                    _logger.LogWarning("Photo not found for unlike operation. Photo Id: {PhotoId}", Id);
                    return;
                }

                var photoHeart = await _databaseContext.PhotoHearts.SingleOrDefaultAsync(p => p.Id_User == Id_User && p.HeartGroup == photo.HeartGroup);
                if (photoHeart == null)
                {
                    _logger.LogWarning("No like found to remove for user {UserId} on photo with HeartGroup: {HeartGroup}", Id_User, photo.HeartGroup);
                    return;
                }

                _databaseContext.PhotoHearts.Remove(photoHeart);
                await _databaseContext.SaveChangesAsync();
                _logger.LogInformation("Photo unliked successfully by user {UserId}", Id_User);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while unliking photo with Id: {PhotoId} by user {UserId}", Id, Id_User);
            }
        }


        public async Task AddComment(string Id_User, string CommentsGroup, string Text)
        {
            _logger.LogInformation("Starting AddComment. User: {UserId}, CommentsGroup: {CommentsGroup}", Id_User, CommentsGroup);

            int count = _databaseContext.PhotoComments.Count(p => p.Id_User == Id_User && p.CommentsGroup == CommentsGroup);

            if (count == 0)
            {
                _logger.LogInformation("No existing comment found for user {UserId} in CommentsGroup {CommentsGroup}, adding new comment.", Id_User, CommentsGroup);

                CreatorPhoto photo = _databaseContext.CreatorPhoto.Single(p => p.CommentsGroup == CommentsGroup);
                UserModel user = _databaseContext.Users.Find(Id_User);

                if (photo == null || user == null)
                {
                    _logger.LogWarning("Photo or user not found. User: {UserId}, CommentsGroup: {CommentsGroup}", Id_User, CommentsGroup);
                    return;
                }

                PhotoComments photoComment = new PhotoComments()
                {
                    Id_User = Id_User,
                    User = user.UserName,
                    CommentsGroup = CommentsGroup,
                    Date = DateOnly.FromDateTime(DateTime.Now),
                    Time = TimeOnly.FromDateTime(DateTime.Now),
                    Hidden = false,
                    Text = Text,
                    CreatorPhoto = photo
                };

                _databaseContext.PhotoComments.Add(photoComment);
                _databaseContext.SaveChanges();

                _logger.LogInformation("Comment added successfully by user {UserId} to CommentsGroup {CommentsGroup}", Id_User, CommentsGroup);
            }
            else
            {
                _logger.LogWarning("User {UserId} has already commented in CommentsGroup {CommentsGroup}", Id_User, CommentsGroup);
            }
        }

        public async Task DeleteComment(int Id, UserModel user)
        {
            _logger.LogInformation("Starting DeleteComment. Comment ID: {CommentId}, User: {UserId}", Id, user.Id);

            int count = _databaseContext.PhotoComments.Count(p => p.Id == Id);

            if (count > 0)
            {
                _logger.LogInformation("Comment found. Comment ID: {CommentId}, proceeding with deletion", Id);

                PhotoComments comment = _databaseContext.PhotoComments.Find(Id);
                if (comment.Id_User == user.Id)
                {
                    _databaseContext.PhotoComments.Remove(comment);
                    _databaseContext.SaveChanges();
                    _logger.LogInformation("Comment deleted successfully. Comment ID: {CommentId}, User: {UserId}", Id, user.Id);
                }
                else
                {
                    _logger.LogWarning("User {UserId} attempted to delete a comment that they do not own. Comment ID: {CommentId}", user.Id, Id);
                }
            }
            else
            {
                _logger.LogWarning("Comment not found. Comment ID: {CommentId}, unable to delete.", Id);
            }
        }

    }
}
