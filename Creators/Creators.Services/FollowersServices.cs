using Creators.Creators.Database;
using Creators.Creators.Services.Interface;

namespace Creators.Creators.Services
{
    public class FollowersServices : IFollow, IGetFollowers
    {
        private readonly DatabaseContext _databaseContext;
        public FollowersServices(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<List<Followers>> GetCreatorFollowers(string Id_Creator)
        {
            return _databaseContext.Followers.Where(p => p.Id_Creator == Id_Creator).ToList();
        }
        public async Task<List<Followers>> GetUserFollowing(UserModel user)
        {
            return _databaseContext.Followers.Where(p => p.Id_User == user.Id).ToList();
        }

        public async Task Follow(string Id_Creator, UserModel user)
        {
            UserModel creator = _databaseContext.Users.Find(Id_Creator); 

            Followers follower = new Followers
            {
                CreatorName = creator.UserName,
                ProfileName = user.UserName,
                Id_Creator = Id_Creator,
                Id_User = user.Id,
                Since = DateTime.Now,

                UserModel = user
            };
            _databaseContext.Followers.Add(follower);
            _databaseContext.SaveChanges();

        }
        public async Task UnFollow(string Id_Creator, UserModel user)
        {
            Followers follower = _databaseContext.Followers.Single(p => p.Id_Creator == Id_Creator && p.Id_User == user.Id);
            _databaseContext.Followers.Remove(follower);
            _databaseContext.SaveChanges();
        }
            
        public async Task<bool> IsFollowing(string Id_Creator, UserModel user)
        {
            return _databaseContext.Followers.Any(p => p.Id_Creator == Id_Creator && p.Id_User == user.Id);
        }

    }
}
