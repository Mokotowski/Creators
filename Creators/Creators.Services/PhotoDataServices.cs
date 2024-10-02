using Creators.Creators.Database;
using Creators.Creators.Models;
using Creators.Creators.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace Creators.Creators.Services
{
    public class PhotoDataServices : IPhotoDataGet
    {
        private readonly ILogger<PhotoDataServices> _logger;
        private readonly DatabaseContext _databaseContext;

        public PhotoDataServices(ILogger<PhotoDataServices> logger, DatabaseContext databaseContext)
        {
            _logger = logger;
            _databaseContext = databaseContext;
        }

        public async Task<List<PhotoHearts>> GetLikesCreator(int Id, UserModel user)
        {
            _logger.LogInformation("Starting GetLikesCreator for user {UserId} and photo {PhotoId}", user.Id, Id);

            var photo = await _databaseContext.CreatorPhoto.FindAsync(Id);
            var creatorPage = await _databaseContext.CreatorPage.FindAsync(user.Id);

            if (creatorPage == null || photo == null)
            {
                _logger.LogWarning("CreatorPage or photo not found for user {UserId} and photo {PhotoId}", user.Id, Id);
                return new List<PhotoHearts>();
            }

            if (creatorPage.Id_Photos == photo.Id_Photos)
            {
                var hearts = await _databaseContext.PhotoHearts
                    .Where(p => p.HeartGroup == photo.HeartGroup)
                    .ToListAsync();
                return hearts;
            }

            return new List<PhotoHearts>();
        }

        public async Task<List<PhotoComments>> GetCommentsCreator(int Id, UserModel user)
        {
            _logger.LogInformation("Starting GetCommentsCreator for user {UserId} and photo {PhotoId}", user.Id, Id);

            var photo = await _databaseContext.CreatorPhoto.FindAsync(Id);
            var creatorPage = await _databaseContext.CreatorPage.FindAsync(user.Id);

            if (creatorPage == null || photo == null)
            {
                _logger.LogWarning("CreatorPage or photo not found for user {UserId} and photo {PhotoId}", user.Id, Id);
                return new List<PhotoComments>();
            }

            if (creatorPage.Id_Photos == photo.Id_Photos)
            {
                var comments = await _databaseContext.PhotoComments
                    .Where(p => p.CommentsGroup == photo.CommentsGroup)
                    .ToListAsync();
                return comments;
            }

            return new List<PhotoComments>();
        }

        public async Task<List<HeartsForUser>> GetLikesUser(int Id)
        {
            _logger.LogInformation("Starting GetLikesUser for photo {PhotoId}", Id);

            var photo = await _databaseContext.CreatorPhoto.FindAsync(Id);
            if (photo == null)
            {
                _logger.LogWarning("Photo with Id {PhotoId} not found", Id);
                return new List<HeartsForUser>();
            }

            var hearts = await _databaseContext.PhotoHearts
                .Where(p => p.HeartGroup == photo.HeartGroup)
                .ToListAsync();

            var heartsForUsers = new List<HeartsForUser>();

            foreach (var heart in hearts)
            {
                var user = await _databaseContext.Users.FindAsync(heart.Id_User);
                if (user != null)
                {
                    heartsForUsers.Add(new HeartsForUser { User = user.UserName });
                }
            }

            return heartsForUsers;
        }

        public async Task<List<CommentsForUser>> GetCommentsUser(int Id, UserModel user)
        {
            _logger.LogInformation("Starting GetCommentsUser for user {UserId} and photo {PhotoId}", user.Id, Id);

            var photo = await _databaseContext.CreatorPhoto.FindAsync(Id);
            if (photo == null)
            {
                _logger.LogWarning("Photo with Id {PhotoId} not found", Id);
                return new List<CommentsForUser>();
            }

            var comments = await _databaseContext.PhotoComments
                .Where(p => p.CommentsGroup == photo.CommentsGroup)
                .ToListAsync();

            var commentsForUsers = new List<CommentsForUser>();

            foreach (var comment in comments)
            {
                if (!comment.Hidden || comment.Id_User == user.Id)
                {
                    var commentUser = await _databaseContext.Users.FindAsync(comment.Id_User);
                    if (commentUser != null)
                    {
                        commentsForUsers.Add(new CommentsForUser
                        {
                            Id = comment.Id,
                            Id_User = comment.Id_User,
                            Date = comment.Date,
                            Time = comment.Time,
                            Text = comment.Text,
                            User = commentUser.UserName
                        });
                    }
                }
            }

            return commentsForUsers;
        }

        public async Task<List<PhotoForCreator>> GetPhotosCreator(UserModel user)
        {
            _logger.LogInformation("Starting GetPhotosCreator for user {UserId}", user.Id);

            var creatorPage = await _databaseContext.CreatorPage.FindAsync(user.Id);
            if (creatorPage == null)
            {
                _logger.LogWarning("CreatorPage not found for user {UserId}", user.Id);
                return new List<PhotoForCreator>();
            }

            var creatorPhotos = await _databaseContext.CreatorPhoto
                .Where(p => p.Id_Photos == creatorPage.Id_Photos)
                .ToListAsync();

            var photos = new List<PhotoForCreator>();

            foreach (var creatorPhoto in creatorPhotos)
            {
                var photoForCreator = new PhotoForCreator
                {
                    Id = creatorPhoto.Id,
                    Id_Photos = creatorPhoto.Id_Photos,
                    CommentsGroup = creatorPhoto.CommentsGroup,
                    HeartGroup = creatorPhoto.HeartGroup,
                    Description = creatorPhoto.Description,
                    CommentsOpen = creatorPhoto.CommentsOpen,
                    File = creatorPhoto.File,
                    FileExtension = creatorPhoto.FileExtension,
                    DateTime = creatorPhoto.DateTime,
                    comments = await _databaseContext.PhotoComments
                        .Where(p => p.CommentsGroup == creatorPhoto.CommentsGroup)
                        .ToListAsync(),
                    Hearts = await _databaseContext.PhotoHearts
                        .Where(p => p.HeartGroup == creatorPhoto.HeartGroup)
                        .ToListAsync()
                };

                photos.Add(photoForCreator);
            }

            return photos;
        }

        public async Task<List<PhotoForUser>> GetCreatorsPhotosUser(UserModel user)
        {
            _logger.LogInformation("Starting GetCreatorsPhotosUser for user {UserId}", user.Id);

            var Id_Photos = await _databaseContext.Followers
                .Where(f => f.Id_User == user.Id)
                .Join(_databaseContext.CreatorPage,
                    follower => follower.Id_Creator,
                    creatorPage => creatorPage.Id_Creator,
                    (follower, creatorPage) => new { creatorPage.Id_Photos })
                .Join(_databaseContext.CreatorPhoto,
                    creatorPage => creatorPage.Id_Photos,
                    creatorPhoto => creatorPhoto.Id_Photos,
                    (creatorPage, creatorPhoto) => creatorPhoto.Id_Photos)
                .ToListAsync();

            var creatorPhotos = await _databaseContext.CreatorPhoto
                .Where(photo => Id_Photos.Contains(photo.Id_Photos))
                .ToListAsync();

            var photos = new List<PhotoForUser>();

            foreach (var creatorPhoto in creatorPhotos)
            {
                var creatorPage = await _databaseContext.CreatorPage
                    .SingleAsync(p => p.Id_Photos == creatorPhoto.Id_Photos);
                var creator = await _databaseContext.Users.FindAsync(creatorPage.Id_Creator);

                var photoForUser = new PhotoForUser
                {
                    Id = creatorPhoto.Id,
                    Description = creatorPhoto.Description,
                    CommentsOpen = creatorPhoto.CommentsOpen,
                    File = creatorPhoto.File,
                    FileExtension = creatorPhoto.FileExtension,
                    DateTime = creatorPhoto.DateTime,
                    Creator = creator?.UserName,
                    GiveLike = await _databaseContext.PhotoHearts
                        .AnyAsync(p => p.HeartGroup == creatorPhoto.HeartGroup && p.Id_User == user.Id),
                    GiveComment = await _databaseContext.PhotoComments
                        .AnyAsync(p => p.CommentsGroup == creatorPhoto.CommentsGroup && p.Id_User == user.Id),
                    CountLike = await _databaseContext.PhotoHearts
                        .CountAsync(p => p.HeartGroup == creatorPhoto.HeartGroup),
                    CommentsGroup = creatorPhoto.CommentsGroup,
                    HeartGroup = creatorPhoto.HeartGroup,
                    CommentsForUsers = await GetCommentsUser(creatorPhoto.Id, user)
                };

                photos.Add(photoForUser);
            }

            return photos;
        }

        public async Task<List<PhotoForUser>> GetCreatorPhotosUser(string Id_Photos, UserModel user)
        {
            _logger.LogInformation("Starting GetCreatorPhotosUser for Id_Photos {Id_Photos} and user {UserId}", Id_Photos, user.Id);

            var creatorPhotos = await _databaseContext.CreatorPhoto
                .Where(p => p.Id_Photos == Id_Photos)
                .ToListAsync();

            var creatorPage = await _databaseContext.CreatorPage
                .SingleAsync(p => p.Id_Photos == Id_Photos);

            var creator = await _databaseContext.Users.FindAsync(creatorPage.Id_Creator);
            var photos = new List<PhotoForUser>();

            foreach (var creatorPhoto in creatorPhotos)
            {
                var photoForUser = new PhotoForUser
                {
                    Id = creatorPhoto.Id,
                    Description = creatorPhoto.Description,
                    CommentsOpen = creatorPhoto.CommentsOpen,
                    File = creatorPhoto.File,
                    FileExtension = creatorPhoto.FileExtension,
                    DateTime = creatorPhoto.DateTime,
                    Creator = creator?.UserName,
                    GiveLike = await _databaseContext.PhotoHearts
                        .AnyAsync(p => p.HeartGroup == creatorPhoto.HeartGroup && p.Id_User == user.Id),
                    GiveComment = await _databaseContext.PhotoComments
                        .AnyAsync(p => p.CommentsGroup == creatorPhoto.CommentsGroup && p.Id_User == user.Id),
                    CountLike = await _databaseContext.PhotoHearts
                        .CountAsync(p => p.HeartGroup == creatorPhoto.HeartGroup),
                    CommentsGroup = creatorPhoto.CommentsGroup,
                    HeartGroup = creatorPhoto.HeartGroup,
                    CommentsForUsers = await GetCommentsUser(creatorPhoto.Id, user)
                };

                photos.Add(photoForUser);
            }

            return photos;
        }


        public async Task<string> GetId_Photos(UserModel user)
        {
            try
            {
                if (user == null || string.IsNullOrEmpty(user.Id))
                {
                    _logger.LogWarning("GetId_Photos: Invalid user provided.");
                    return null;
                }

                _logger.LogInformation("GetId_Photos: Retrieving Id_Photos for user {UserId}", user.Id);

                CreatorPage page = await _databaseContext.CreatorPage.FindAsync(user.Id);

                if (page == null)
                {
                    _logger.LogWarning("GetId_Photos: CreatorPage not found for user {UserId}", user.Id);
                    return null;
                }

                _logger.LogInformation("GetId_Photos: Successfully retrieved Id_Photos for user {UserId}", user.Id);
                return page.Id_Photos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetId_Photos: Error retrieving Id_Photos for user {UserId}", user?.Id);
                return null;
            }
        }


    }
}
