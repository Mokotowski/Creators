using Creators.Creators.Database;

namespace Creators.Creators.Services.Interface
{
    public interface IChatsActions
    {
        public Task<List<Chats>> GetChats(UserModel user);
        public Task<(Chats, string)> GetChat(int Id, UserModel user);

        public Task<Chats> AddMessage(int Id, UserModel user, string  Text);
        public Task<Chats> DeleteMessage(int Id, UserModel user);
        public Task<Chats> MakeChat(UserModel user_me, string user_friend);
    }
}
