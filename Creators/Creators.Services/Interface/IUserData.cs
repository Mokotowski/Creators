using Creators.Creators.Database;
using Creators.Creators.Models;

namespace Creators.Creators.Services.Interface
{
    public interface IUserData
    {
        public Task<List<UserForChats>> GetUsers(string User, UserModel user);
    }
}
