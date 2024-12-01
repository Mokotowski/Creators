using Creators.Creators.Database;
using Creators.Creators.Models;

namespace Creators.Creators.Services.Interface
{
    public interface IPageFunctions
    {
        public Task<string> CreatePage(UserModel User, string AccountNumber, string Description, byte[] ProfileImage, string ProfileImageExtension, bool NotifyImages, bool NotifyEvents, string BioLinks);
        public Task<string> UpdatePage(UserModel User, string Description, byte[]? ProfileImage, string? ProfileImageExtension, bool NotifyImages, bool NotifyEvents, string BioLinks);
        public Task<string> UpdatePageAccount(UserModel user, string AccountNumber);
        public Task<CreatorPageShow> GetPageForUsers(string Id_Creator);
        public Task<CreatorPageShow> GetPageForUpdate(string Id_Creator);
        public Task<List<CreatorsSearch>> FindCreators(string ProfileName);
    }
}
 