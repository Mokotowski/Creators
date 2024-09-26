using Creators.Creators.Database;
using Creators.Creators.Models;
using Creators.Creators.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

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
            CreatorPhoto photo = _databaseContext.CreatorPhoto.Find(Id);
            CreatorPage creatorPage = _databaseContext.CreatorPage.Find(user.Id);
            
            if(creatorPage.Id_Photos == photo.Id_Photos)
            {
                List<PhotoHearts> hearts = _databaseContext.PhotoHearts.Where(p => p.HeartGroup == photo.HeartGroup).ToList();
                return hearts;
            }

            return new List<PhotoHearts>(); 
        }
        public async Task<List<PhotoComments>> GetCommentsCreator(int Id, UserModel user)
        {
            CreatorPhoto photo = _databaseContext.CreatorPhoto.Find(Id);
            CreatorPage creatorPage = _databaseContext.CreatorPage.Find(user.Id);

            if (creatorPage.Id_Photos == photo.Id_Photos)
            {
                List<PhotoComments> comments = _databaseContext.PhotoComments.Where(p => p.CommentsGroup == photo.CommentsGroup).ToList();
                return comments;
            }

            return new List<PhotoComments>();
        }

        public async Task<List<HeartsForUser>> GetLikesUser(int Id)
        {
            CreatorPhoto photo = _databaseContext.CreatorPhoto.Find(Id);
            List <PhotoHearts> hearts = _databaseContext.PhotoHearts.Where(p => p.HeartGroup == photo.HeartGroup).ToList();
            List<HeartsForUser> heartsForUsers = new List<HeartsForUser>();

            for (int i = 0; i < hearts.Count; i++)
            {
                HeartsForUser heart = new HeartsForUser()
                {
                    User = _databaseContext.Users.Find(hearts[i].Id).UserName
                };
                heartsForUsers.Add(heart);
            }

            return heartsForUsers;  
        }
        public async Task<List<CommentsForUser>> GetCommentsUser(int Id, UserModel user)
        {
            CreatorPhoto photo = _databaseContext.CreatorPhoto.Find(Id);
            List<PhotoComments> comments = _databaseContext.PhotoComments.Where(p => p.CommentsGroup == photo.CommentsGroup).ToList();
            List<CommentsForUser> commentsForUsers = new List<CommentsForUser>();

            for (int i = 0; i < comments.Count; i++)
            {
                if (!comments[i].Hidden || comments[i].Id_User == user.Id)
                {
                    CommentsForUser comment = new CommentsForUser()
                    {
                        Date = comments[i].Date,
                        Time = comments[i].Time,
                        Text = comments[i].Text,
                        User = _databaseContext.Users.Find(comments[i].Id).UserName
                    };
                    commentsForUsers.Add(comment);
                }
            }

            return commentsForUsers;
        }


        public async Task<List<PhotoForCreator>> GetPhotosCreator(UserModel user)
        {
            CreatorPage creatorPage = _databaseContext.CreatorPage.Find(user.Id);
            List<CreatorPhoto> creatorPhotos = _databaseContext.CreatorPhoto.Where(p => p.Id_Photos == creatorPage.Id_Photos).ToList();
            List<PhotoForCreator> photos = new List<PhotoForCreator>();

            for (int i = 0; i < creatorPhotos.Count; i++)
            {
                PhotoForCreator photoForCreator = new PhotoForCreator()
                {
                    Id = creatorPhotos[i].Id,
                    Id_Photos = creatorPhotos[i].Id_Photos,
                    CommentsGroup = creatorPhotos[i].CommentsGroup,
                    HeartGroup = creatorPhotos[i].HeartGroup,
                    Description = creatorPhotos[i].Description,
                    CommentsOpen = creatorPhotos[i].CommentsOpen,
                    File = creatorPhotos[i].File,
                    FileExtension = creatorPhotos[i].FileExtension,
                    DateTime = creatorPhotos[i].DateTime,

                    comments = _databaseContext.PhotoComments.Where(p => p.CommentsGroup == creatorPhotos[i].CommentsGroup).ToList(),
                    Hearts = _databaseContext.PhotoHearts.Where(p => p.HeartGroup == creatorPhotos[i].HeartGroup).ToList(),
                };

                photos.Add(photoForCreator);
            }

            return photos;
        }


        public async Task<List<PhotoForUser>> GetCreatorsPhotosUser(UserModel user)
        {
            List<string> Id_Photos = await _databaseContext.Followers
                .Where(f => f.Id_User == user.Id)
                .Join(
                    _databaseContext.CreatorPage,
                    follower => follower.Id_Creator,
                    creatorPage => creatorPage.Id_Creator,
                    (follower, creatorPage) => new { creatorPage.Id_Photos } 
                )
                .Join(
                    _databaseContext.CreatorPhoto,
                    creatorPage => creatorPage.Id_Photos, 
                    creatorPhoto => creatorPhoto.Id_Photos,
                    (creatorPage, creatorPhoto) => creatorPhoto.Id_Photos 
                )
                .ToListAsync();

            var creatorPhotos = await _databaseContext.CreatorPhoto
                .Where(photo => Id_Photos.Contains(photo.Id_Photos)) 
                .ToListAsync();

            List<PhotoForUser> photos = new List<PhotoForUser>();

            for (int i = 0; i < creatorPhotos.Count; i++)
            {
                CreatorPage creatorPage = _databaseContext.CreatorPage.Single(p => p.Id_Photos == creatorPhotos[i].Id_Photos);
                UserModel creator = _databaseContext.Users.Find(creatorPage.Id_Creator);


                bool GiveLike = _databaseContext.PhotoHearts.Any(p => p.HeartGroup == p.HeartGroup && p.Id_User == user.Id);
                bool GiveComment = _databaseContext.PhotoComments.Any(p => p.CommentsGroup == p.CommentsGroup && p.Id_User == user.Id);
                int CountLike = _databaseContext.PhotoHearts.Count(p => p.HeartGroup == creatorPhotos[i].HeartGroup);

                PhotoForUser photoForUser = new PhotoForUser()
                {
                    Description = creatorPhotos[i].Description,
                    CommentsOpen = creatorPhotos[i].CommentsOpen,
                    File = creatorPhotos[i].File,
                    FileExtension = creatorPhotos[i].FileExtension,
                    DateTime = creatorPhotos[i].DateTime,
                    Creator = creator.UserName,
                    GiveLike = GiveLike,
                    GiveComment = GiveComment,
                    CountLike = CountLike,

                    CommentsGroup = creatorPhotos[i].CommentsGroup,
                    HeartGroup = creatorPhotos[i].HeartGroup,

                    CommentsForUsers = await GetCommentsUser(creatorPhotos[i].Id, user)
                };

                photos.Add(photoForUser);
            }

            return photos;
        }


        public async Task<List<PhotoForUser>> GetCreatorPhotosUser(string Id_Photos, UserModel user)
        {
            List<CreatorPhoto> creatorPhotos = _databaseContext.CreatorPhoto.Where(p => p.Id_Photos == Id_Photos).ToList();
            CreatorPage creatorPage = _databaseContext.CreatorPage.Single(p => p.Id_Photos == Id_Photos);
            UserModel creator = _databaseContext.Users.Find(creatorPage.Id_Creator);
            List<PhotoForUser> photos = new List<PhotoForUser>();

            for(int i = 0; i < creatorPhotos.Count; i++)
            {
                bool GiveLike = _databaseContext.PhotoHearts.Any(p => p.HeartGroup == p.HeartGroup && p.Id_User == user.Id);
                bool GiveComment = _databaseContext.PhotoComments.Any(p => p.CommentsGroup == p.CommentsGroup && p.Id_User == user.Id);
                int CountLike = _databaseContext.PhotoHearts.Count(p => p.HeartGroup == creatorPhotos[i].HeartGroup);

                PhotoForUser photoForUser = new PhotoForUser()
                {
                    Description = creatorPhotos[i].Description,
                    CommentsOpen = creatorPhotos[i].CommentsOpen,
                    File = creatorPhotos[i].File,
                    FileExtension = creatorPhotos[i].FileExtension,
                    DateTime = creatorPhotos[i].DateTime,
                    Creator = creator.UserName,
                    GiveLike = GiveLike,
                    GiveComment = GiveComment,
                    CountLike = CountLike,

                    CommentsGroup = creatorPhotos[i].CommentsGroup,
                    HeartGroup = creatorPhotos[i].HeartGroup,

                    CommentsForUsers = await GetCommentsUser(creatorPhotos[i].Id, user)
                };

                photos.Add(photoForUser);
            }

            return photos;
        }
    }
}
