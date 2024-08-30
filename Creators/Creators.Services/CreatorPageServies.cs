using Creators.Creators.Database;
using Creators.Creators.Models;
using Creators.Creators.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace Creators.Creators.Services
{
    public class CreatorPageServies : IPageFunctions
    {
        private readonly DatabaseContext _databaseContext;
        private readonly ILogger<CreatorPageServies> _logger;

        public CreatorPageServies(DatabaseContext databaseContext, ILogger<CreatorPageServies> logger)
        {
            _databaseContext = databaseContext;
            _logger = logger;

        }
        public async Task<string> CreatePage(UserModel user, string accountNumber, string description, string profileImage, bool notifyImages, bool notifyEvents, string bioLinks)
        {
            if (user == null)
            {
                _logger.LogWarning("CreatePage called with null user.");
                return "Error";
            }

            if (string.IsNullOrEmpty(accountNumber) || string.IsNullOrEmpty(description))
            {
                _logger.LogWarning("CreatePage called with invalid parameters.");
                return "Error";
            }

            try
            {
                if (!_databaseContext.CreatorPage.Any(p => p.Id_Creator == user.Id))
                {
                    user.IsCreator = true;

                    var page = new CreatorPage
                    {
                        Id_Creator = user.Id,
                        Id_Calendar = user.Id,
                        Id_Photos = user.Id,
                        Id_Donates = user.Id,
                        Account_Numer = accountNumber,
                        Account_Balance = 0,
                        Site_Commission = 0.05m,
                        User = user
                    };

                    var data = new PageData
                    {
                        Id_Creator = user.Id,
                        Description = description,
                        ProfilName = user.UserName,
                        ProfilPicture = profileImage,
                        EmailNotificationsPhoto = notifyImages,
                        EmailNotificationsEvents = notifyEvents,
                        BioLinks = bioLinks,
                        CreatorPage = page
                    };

                    _databaseContext.PageData.Add(data);
                    _databaseContext.CreatorPage.Add(page);
                    await _databaseContext.SaveChangesAsync();

                    _logger.LogInformation("Page created successfully for user {UserId}.", user.Id);
                    return user.Id;
                }
                else
                {
                    _logger.LogInformation("Page creation attempt for user {UserId} failed: page already exists.", user.Id);
                    return "Exist";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a page for user {UserId}.", user.Id);
                return "Error";
            }
        }

        public async Task<string> UpdatePage(UserModel user, string accountNumber, string description, string profileImage, bool notifyImages, bool notifyEvents, string bioLinks)
        {
            if (user == null)
            {
                _logger.LogWarning("UpdatePage called with null user.");
                return "Error";
            }

            if (string.IsNullOrEmpty(accountNumber))
            {
                _logger.LogWarning("UpdatePage called with invalid account number.");
                return "Error";
            }

            if (string.IsNullOrEmpty(description))
            {
                _logger.LogWarning("UpdatePage called with invalid description.");
                return "Error";
            }

            if (string.IsNullOrEmpty(profileImage))
            {
                _logger.LogWarning("UpdatePage called with invalid profile image.");
                return "Error";
            }

            if (string.IsNullOrEmpty(bioLinks))
            {
                _logger.LogWarning("UpdatePage called with invalid bio links.");
                return "Error";
            }


            try
            {
                var cPage = await _databaseContext.CreatorPage.FindAsync(user.Id);
                var dPage = await _databaseContext.PageData.FindAsync(user.Id);

                if (cPage == null || dPage == null)
                {
                    _logger.LogInformation("UpdatePage attempt failed for user {UserId}: page does not exist.", user.Id);
                    return "Exist";
                }

                cPage.Account_Numer = accountNumber;

                dPage.Description = description;
                dPage.ProfilPicture = profileImage;
                dPage.EmailNotificationsPhoto = notifyImages;
                dPage.EmailNotificationsEvents = notifyEvents;
                dPage.BioLinks = bioLinks;

                await _databaseContext.SaveChangesAsync();

                _logger.LogInformation("Page updated successfully for user {UserId}.", user.Id);
                return "Success";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the page for user {UserId}.", user.Id);
                return "Error";
            }
        }


        public async Task<CreatorPageShow> GetPageForUsers(string Id_Creator)
        {
            if (string.IsNullOrWhiteSpace(Id_Creator))
            {
                _logger.LogWarning("Attempted to retrieve page data with an invalid ID.");
                throw new ArgumentException("ID cannot be null or empty.", nameof(Id_Creator));
            }

            try
            {
                _logger.LogInformation("Retrieving page data for user with ID: {Id_Creator}", Id_Creator);

                var pageData = await _databaseContext.PageData.FindAsync(Id_Creator);

                if (pageData == null)
                {
                    _logger.LogWarning("No page data found for ID: {Id_Creator}", Id_Creator);
                    return null;
                }

                var pageShow = new CreatorPageShow(
                    pageData.ProfilName,
                    pageData.Description,
                    pageData.ProfilPicture,
                    pageData.BioLinks,
                    pageData.Id_Creator
                );

                _logger.LogInformation("Successfully retrieved page data for user with ID: {Id_Creator}", Id_Creator);
                return pageShow;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving page data for user with ID: {Id_Creator}", Id_Creator);
                throw;
            }
        }

        public async Task<CreatorPageShow> GetPageForUpdate(string Id_Creator)
        {
            if (string.IsNullOrWhiteSpace(Id_Creator))
            {
                _logger.LogWarning("Attempted to retrieve page data for update with an invalid ID.");
                throw new ArgumentException("ID cannot be null or empty.", nameof(Id_Creator));
            }

            try
            {
                _logger.LogInformation("Retrieving page and creator data for update with ID: {Id_Creator}", Id_Creator);

                var creatorPage = await _databaseContext.CreatorPage.FindAsync(Id_Creator);
                var pageData = await _databaseContext.PageData.FindAsync(Id_Creator);

                if (creatorPage == null)
                {
                    _logger.LogWarning("No creator page data found for ID: {Id_Creator}", Id_Creator);
                    return null;
                }

                if (pageData == null)
                {
                    _logger.LogWarning("No page data found for ID: {Id_Creator}", Id_Creator);
                    return null;
                }

                var pageShow = new CreatorPageShow(
                    creatorPage.Account_Numer,
                    pageData.Description,
                    pageData.ProfilPicture,
                    pageData.BioLinks,
                    pageData.EmailNotificationsPhoto,
                    pageData.EmailNotificationsEvents,
                    pageData.Id_Creator

                );

                _logger.LogInformation("Successfully retrieved page and creator data for update with ID: {Id_Creator}", Id_Creator);
                return pageShow;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving page and creator data for update with ID: {Id_Creator}", Id_Creator);
                throw;
            }
        }


        public async Task<List<CreatorsSearch>> FindCreators(string ProfileName)
        {
            if (string.IsNullOrWhiteSpace(ProfileName))
            {
                _logger.LogWarning("Search phrase is empty or null.");
                return new List<CreatorsSearch>();
            }

            List<PageData> pages = _databaseContext.PageData
                .AsEnumerable()
                .Where(p => p.ProfilName.Contains(ProfileName, StringComparison.OrdinalIgnoreCase))
                .ToList();

            List<CreatorsSearch> search = new List<CreatorsSearch>();

            for (int i = 0; i < pages.Count; i++)
            {
                CreatorsSearch searchItem = new CreatorsSearch(pages[i].Id_Creator, pages[i].ProfilName, pages[i].ProfilPicture);
                search.Add(searchItem);
            }

            _logger.LogInformation("Found {Count} pages containing the phrase '{SearchPhrase}'", pages.Count, ProfileName);

            return search;
        }

    }
}


