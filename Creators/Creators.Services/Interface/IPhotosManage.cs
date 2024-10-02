using Creators.Creators.Database;

namespace Creators.Creators.Services.Interface
{
    public interface IPhotosManage
    {
        public Task AddPhoto(string Description, bool CommentsOpen, byte[] File, string FileExtension, UserModel user);
        public Task DeletePhoto(int Id, UserModel user);
        public Task ChangeVisibleComment(int Id, UserModel user);

    }
}
