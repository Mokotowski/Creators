using Creators.Creators.Database;

namespace Creators.Creators.Services.Interface
{
    public interface IBlock
    {
        public string CheckBlock(string Id_User, UserModel user);
        public Task BlockUser(string Id_User, UserModel user);
        public Task UnblockUser(int Id, UserModel user);
        public Task<List<Blocklist>> GetBlockUsers(UserModel user);
    }
}
