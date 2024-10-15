using Creators.Creators.Database;
using Creators.Creators.Services.Interface;
using System.Linq;

namespace Creators.Creators.Services
{
    public class AnnouncementServices : IAnnouncementManage, IAnnouncementData
    {
        private readonly DatabaseContext _databaseContext;
        private readonly ILogger<AnnouncementServices> _logger;
        public AnnouncementServices(DatabaseContext databaseContext, ILogger<AnnouncementServices> logger)
        {
            _databaseContext = databaseContext;
            _logger = logger;
        }

        public async Task AddAnnouncement(UserModel user, string Title, string Description)
        {
            try
            {
                _logger.LogInformation($"User {user.UserName} is attempting to add an announcement.");

                CreatorPage creatorPage = _databaseContext.CreatorPage.Find(user.Id);
                if (creatorPage == null)
                {
                    _logger.LogWarning($"Creator page not found for user {user.UserName} (Id: {user.Id}).");
                    return;
                }

                CreatorAnnouncement announcement = new CreatorAnnouncement()
                {
                    Creator = user.UserName,
                    Title = Title,
                    Description = Description,
                    Id_Announcement = creatorPage.Id_Announcement,
                    IsUpdated = false,
                    DateTime = DateTime.Now,
                    CreatorPage = creatorPage
                };

                _databaseContext.CreatorAnnouncement.Add(announcement);
                await _databaseContext.SaveChangesAsync();

                _logger.LogInformation($"Announcement added successfully by user {user.UserName}.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error adding announcement for user {user.UserName}.");
                throw; // Rethrow exception after logging.
            }
        }

        public async Task DeleteAnnouncement(UserModel user, int Id)
        {
            try
            {
                _logger.LogInformation($"User {user.UserName} is attempting to delete announcement {Id}.");

                CreatorAnnouncement announcement = _databaseContext.CreatorAnnouncement.Find(Id);
                if (announcement == null)
                {
                    _logger.LogWarning($"Announcement {Id} not found.");
                    return;
                }

                CreatorPage creatorPage = _databaseContext.CreatorPage.Find(user.Id);
                if (creatorPage == null || announcement.Id_Announcement != creatorPage.Id_Announcement)
                {
                    _logger.LogWarning($"User {user.UserName} is not authorized to delete this announcement.");
                    return;
                }

                _databaseContext.CreatorAnnouncement.Remove(announcement);
                await _databaseContext.SaveChangesAsync();

                _logger.LogInformation($"Announcement {Id} deleted successfully by user {user.UserName}.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting announcement {Id} for user {user.UserName}.");
                throw;
            }
        }

        public async Task UpdateAnnouncement(UserModel user, int Id, string Title, string Description)
        {
            try
            {
                _logger.LogInformation($"User {user.UserName} is attempting to update announcement {Id}.");

                CreatorAnnouncement announcement = _databaseContext.CreatorAnnouncement.Find(Id);
                if (announcement == null)
                {
                    _logger.LogWarning($"Announcement {Id} not found.");
                    return;
                }

                CreatorPage creatorPage = _databaseContext.CreatorPage.Find(user.Id);
                if (creatorPage == null || announcement.Id_Announcement != creatorPage.Id_Announcement)
                {
                    _logger.LogWarning($"User {user.UserName} is not authorized to update this announcement.");
                    return;
                }

                announcement.Title = Title;
                announcement.Description = Description;
                announcement.IsUpdated = true;
                await _databaseContext.SaveChangesAsync();

                _logger.LogInformation($"Announcement {Id} updated successfully by user {user.UserName}.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating announcement {Id} for user {user.UserName}.");
                throw;
            }
        }

        public async Task<List<CreatorAnnouncement>> MyAnnouncement(UserModel user)
        {
            try
            {
                _logger.LogInformation($"User {user.UserName} is retrieving their announcements.");

                CreatorPage creatorPage = _databaseContext.CreatorPage.Find(user.Id);
                if (creatorPage == null)
                {
                    _logger.LogWarning($"Creator page not found for user {user.UserName}.");
                    return new List<CreatorAnnouncement>();
                }

                return _databaseContext.CreatorAnnouncement
                    .Where(p => p.Id_Announcement == creatorPage.Id_Announcement)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving announcements for user {user.UserName}.");
                throw;
            }
        }

        public async Task<List<CreatorAnnouncement>> CreatorAnnouncements(string Id_Announcement)
        {
            try
            {
                _logger.LogInformation($"Retrieving announcements for Id_Announcement: {Id_Announcement}.");

                return _databaseContext.CreatorAnnouncement
                    .Where(p => p.Id_Announcement == Id_Announcement)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving announcements for Id_Announcement: {Id_Announcement}.");
                throw;
            }
        }

        public async Task<List<CreatorAnnouncement>> MyCreatorsAnnouncement(UserModel user)
        {
            try
            {
                _logger.LogInformation($"User {user.UserName} is retrieving announcements from creators they follow.");

                List<string> Id_Creators = _databaseContext.Followers
                    .Where(p => p.Id_User == user.Id)
                    .Select(p => p.Id_Creator)
                    .ToList();

                List<string> Id_Announcements = _databaseContext.CreatorPage
                    .Where(p => Id_Creators.Contains(p.Id_Creator))
                    .Select(p => p.Id_Announcement)
                    .ToList();

                return _databaseContext.CreatorAnnouncement
                    .Where(p => Id_Announcements.Contains(p.Id_Announcement))
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving followed creators' announcements for user {user.UserName}.");
                throw;
            }
        }
    }
}
