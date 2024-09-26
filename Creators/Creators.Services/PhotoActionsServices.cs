using Creators.Creators.Database;
using Creators.Creators.Services.Interface;

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
            CreatorPhoto photo = _databaseContext.CreatorPhoto.Single(p => p.HeartGroup == HeartGroup);
            PhotoHearts photoHeart = new PhotoHearts()
            {
                Id_User = Id_User,
                HeartGroup = HeartGroup,

                CreatorPhoto = photo
            };

            _databaseContext.PhotoHearts.Add(photoHeart);
            _databaseContext.SaveChanges();
        }

        public async Task UnLinkePhoto(int Id)
        {
            PhotoHearts photoHeart = await _databaseContext.PhotoHearts.FindAsync(Id); 
            _databaseContext.PhotoHearts.Remove(photoHeart);
            _databaseContext.SaveChanges();
        }



        public async Task AddComment(string Id_User, string CommentsGroup, string Text)
        {
            CreatorPhoto photo = _databaseContext.CreatorPhoto.Single(p => p.CommentsGroup == CommentsGroup);
            PhotoComments photoComment = new PhotoComments()
            {
                Id_User = Id_User,
                CommentsGroup = CommentsGroup,
                Date = DateOnly.FromDateTime(DateTime.Now),
                Time = TimeOnly.FromDateTime(DateTime.Now),
                Hidden = false,
                Text = Text,

                CreatorPhoto  = photo
            };

            _databaseContext.PhotoComments.Add(photoComment);
            _databaseContext.SaveChanges();
        }

        public async Task DeleteComment(int Id, UserModel user)
        {
             int count = _databaseContext.PhotoComments.Count(p => p.Id == Id);
            if(count > 0)
            {
                PhotoComments comment = _databaseContext.PhotoComments.Find(Id);
                if (comment.Id_User == user.Id)
                {
                    _databaseContext.PhotoComments.Remove(comment);
                    _databaseContext.SaveChanges();
                }
            }
        
        }
    }
}
