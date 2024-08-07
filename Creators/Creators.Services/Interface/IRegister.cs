namespace Creators.Creators.Services.Interface
{
    public interface IRegister
    {
        public Task<bool> SimpleRegister(string Nick, string FirstName, string LastName, string PhoneNumber, string Email, string Password);
        public Task ExternalRegister(string type);

    }
}
