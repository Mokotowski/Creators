namespace Creators.Creators.Services.Interface
{
    public interface ISendEmail
    {
        public Task SendConfirmedEmail(string Email);
        public Task SendResetPasswordEmail(string EMail);   
    }
}
