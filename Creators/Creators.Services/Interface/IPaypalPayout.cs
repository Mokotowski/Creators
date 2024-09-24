using Creators.Creators.Database;

namespace Creators.Creators.Services.Interface
{
    public interface IPaypalPayout
    {
        Task<string> CreatePayout(decimal amount, UserModel user);
    }
}
 