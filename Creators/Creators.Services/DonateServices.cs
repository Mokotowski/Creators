using Creators.Creators.Database;
using Creators.Creators.Models;
using Creators.Creators.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Creators.Creators.Services
{
    public class DonateServices : IDonatesInfo
    {
        private readonly DatabaseContext _databaseContext;
        private readonly ILogger<DonateServices> _logger;

        public DonateServices(DatabaseContext databaseContext, ILogger<DonateServices> logger)
        {
            _databaseContext = databaseContext;
            _logger = logger;
        }

        public async Task<List<DonateCreatorForView>> GetDonates(string Id_Donates)
        {
            _logger.LogInformation("Fetching donations for Id_Donates: {Id_Donates}", Id_Donates);

            try
            {
                var donations = await _databaseContext.Donates
                    .Where(d => d.Id_Donates == Id_Donates)
                    .ToListAsync();

                _logger.LogInformation("Retrieved {Count} donations for Id_Donates: {Id_Donates}", donations.Count, Id_Donates);

                var donateForViews = donations.Select(d => new DonateCreatorForView(
                    donator: _databaseContext.Users.FirstOrDefault(u => u.Id == d.Donator)?.UserName ?? "Unknown",
                    dateTime: d.DateTime,
                    count: d.Count,
                    currency: d.Currency
                )).ToList();

                return donateForViews;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching donations for Id_Donates: {Id_Donates}", Id_Donates);
                throw;
            }
        }

        public async Task<CreatorBalance> GetCreatorBalance(string Id_Donates)
        {
            _logger.LogInformation("Fetching creator balance for Id_Donates: {Id_Donates}", Id_Donates);

            try
            {
                var balance = await _databaseContext.CreatorBalance.FindAsync(Id_Donates);

                if (balance == null)
                {
                    _logger.LogWarning("No creator balance found for Id_Donates: {Id_Donates}", Id_Donates);
                }
                else
                {
                    _logger.LogInformation("Retrieved creator balance for Id_Donates: {Id_Donates}", Id_Donates);
                }

                return balance;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching creator balance for Id_Donates: {Id_Donates}", Id_Donates);
                throw;
            }
        }

        public async Task<bool> CheckPayoutPossibility(decimal amount, UserModel user)
        {
            _logger.LogInformation("Checking payout possibility for user {UserId} with amount: {Amount}", user.Id, amount);

            if (amount <= 0)
            {
                _logger.LogWarning("Amount must be greater than zero.");
                return false;
            }

            if (!user.EmailConfirmed)
            {
                _logger.LogWarning("User {UserId} has not confirmed their email.", user.Id);
                return false;
            }

            if (!user.IsCreator)
            {
                _logger.LogWarning("User {UserId} is not a creator, cannot perform payout.", user.Id);
                return false;
            }

            try
            {
                var balance = await _databaseContext.CreatorBalance
                    .SingleOrDefaultAsync(p => p.Id_Creator == user.Id);

                if (balance == null)
                {
                    _logger.LogError("No balance found for creator {UserId}.", user.Id);
                    return false;
                }

                if (balance.Balance >= amount)
                {
                    _logger.LogInformation("Payout is possible for user {UserId}, amount: {Amount}, available balance: {Balance}", user.Id, amount, balance.Balance);
                    return true;
                }
                else
                {
                    _logger.LogWarning("Insufficient balance for user {UserId}. Requested: {Amount}, Available: {Balance}", user.Id, amount, balance.Balance);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while checking payout possibility for user {UserId}.", user.Id);
                throw;
            }
        }

        public async Task<List<DonateUserForView>> CheckSendDonates(string Donator)
        {
            _logger.LogInformation("Checking donations for Donator: {Donator}", Donator);

            try
            {
                var donations = await _databaseContext.Donates
                    .Where(d => d.Donator == Donator)
                    .ToListAsync();

                _logger.LogInformation("Retrieved {Count} donations for Donator: {Donator}", donations.Count, Donator);

                var donateForViews = donations.Select(d => new DonateUserForView(
                    creator: _databaseContext.Users.FirstOrDefault(u => u.Id == d.Id_Donates)?.UserName ?? "Unknown",
                    dateTime: d.DateTime,
                    count: d.Count,
                    currency: d.Currency
                )).ToList();

                if (donateForViews.Count > 0)
                {
                    _logger.LogInformation("Found {Count} donations for Donator: {Donator}", donateForViews.Count, Donator);
                }
                else
                {
                    _logger.LogWarning("No donations found for Donator: {Donator}", Donator);
                }

                return donateForViews;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while checking donations for Donator: {Donator}", Donator);
                return new List<DonateUserForView>();
            }
        }
    }
}
