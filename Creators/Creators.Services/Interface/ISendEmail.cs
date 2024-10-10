using Creators.Creators.Database;

namespace Creators.Creators.Services.Interface
{
    public interface ISendEmail
    {
        public Task SendConfirmedEmail(string Email);
        public Task SendResetPasswordEmail(string EMail);
        public Task UpdatePage(UserModel User, string AccountNumber);
    }
}
