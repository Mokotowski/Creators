using Creators.Creators.Database;

namespace Creators.Creators.Services.Interface
{
    public interface IAnnouncementManage
    {
        public Task AddAnnouncement(UserModel user, string Title, string Description);

        public Task DeleteAnnouncement(UserModel user, int Id);
        public Task UpdateAnnouncement(UserModel user, int Id, string Title, string Description);
        public Task<List<CreatorAnnouncement>> MyAnnouncement(UserModel user);
    }
}
