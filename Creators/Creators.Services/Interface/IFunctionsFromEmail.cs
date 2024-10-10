using Creators.Creators.Database;

namespace Creators.Creators.Services.Interface
{
    public interface IFunctionsFromEmail
    {
        public Task ConfirmedEmail(string code, string Email);

        public Task ResetPassword(string code, string Email, string password);

        public Task<string> UpdatePageFinalizator(string UserFromEmailId, string AccountNumber, UserModel ActualUser);
    }
}
