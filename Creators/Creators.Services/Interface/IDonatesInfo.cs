using Creators.Creators.Database;
using Creators.Creators.Models;

namespace Creators.Creators.Services.Interface
{
    public interface IDonatesInfo
    {
        public Task<List<DonateCreatorForView>> GetDonates(string Id_Donates);
        public Task<CreatorBalance> GetCreatorBalance(string Id_Donates);
        public Task<bool> CheckPayoutPossibility(decimal amount, UserModel user);
        public Task<List<DonateUserForView>> CheckSendDonates(string Donator);    


    }
}
