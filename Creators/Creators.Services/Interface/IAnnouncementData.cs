using Creators.Creators.Database;

namespace Creators.Creators.Services.Interface
{
    public interface IAnnouncementData
    {
        public Task<List<CreatorAnnouncement>> CreatorAnnouncements(string Id_Announcement);
        public Task<List<CreatorAnnouncement>> MyCreatorsAnnouncement(UserModel user);
    }
}
