using Creators.Creators.Database;

namespace Creators.Creators.Services.Interface
{
    public interface IFollow
    {
        public Task Follow(string Id_Creator, UserModel user);
        public Task UnFollow(string Id_Creator, UserModel user);
        public Task <bool> IsFollowing(string Id_Creator, UserModel user);
    }
}
