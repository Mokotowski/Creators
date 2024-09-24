using Creators.Creators.Database;
using PayPal.Api;

namespace Creators.Creators.Services.Interface
{
    public interface IPaypalPay
    {
        public Task<Payment> CreatePayment(int amount, string returnUrl, string cancelUrl, string creator, string currency);
        public Task PaymentSuccessful(string paymentId, string token, UserModel user, string Id_Donates);
    }
}
 