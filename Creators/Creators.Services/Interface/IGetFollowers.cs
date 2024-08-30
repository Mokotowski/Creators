using Creators.Creators.Database;

namespace Creators.Creators.Services.Interface
{
    public interface IGetFollowers
    {
        public Task<List<Followers>> GetCreatorFollowers(string Id_Creator);
        public Task<List<Followers>> GetUserFollowing(UserModel user);   
    }
}
