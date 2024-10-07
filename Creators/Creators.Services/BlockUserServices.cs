using Creators.Creators.Database;
using Creators.Creators.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging; // Corrected the namespace

namespace Creators.Creators.Services
{
    public class BlockUserServices : IBlock
    {
        private readonly DatabaseContext _databaseContext;
        private readonly ILogger<BlockUserServices> _logger;

        public BlockUserServices(DatabaseContext databaseContext, ILogger<BlockUserServices> logger)
        {
            _databaseContext = databaseContext;
            _logger = logger;
        }

        public string CheckBlock(string Id_User, UserModel user)
        {
            try
            {
                _logger.LogInformation("Checking block status between user {UserId} and user {Id_User}", user.Id, Id_User);

                var blockStatus = _databaseContext.Blocklist
                    .Where(p => (p.Id_BlockUser == Id_User && p.Id_User == user.Id) ||
                                (p.Id_BlockUser == user.Id && p.Id_User == Id_User))
                    .Select(p => new { Block = p.Id_BlockUser == Id_User, IsBlocked = p.Id_BlockUser == user.Id })
                    .FirstOrDefault();

                if (blockStatus?.Block == true)
                    return "Block";

                if (blockStatus?.IsBlocked == true)
                    return "IsBlocked";

                return "Not";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while checking block status for user {UserId}", user.Id);
                throw;
            }
        }

        public async Task BlockUser(string Id_User, UserModel user)
        {
            try
            {
                _logger.LogInformation("Attempting to block user {Id_User} by user {UserId}", Id_User, user.Id);

                if (!_databaseContext.Blocklist.Any(p => p.Id_User == user.Id && p.Id_BlockUser == Id_User))
                {
                    var userblocked = await _databaseContext.Users.FindAsync(Id_User);

                    if (userblocked != null)
                    {
                        var blocklist = new Blocklist
                        {
                            Id_User = user.Id,
                            Id_BlockUser = Id_User,
                            UserName = userblocked.UserName
                        };

                        _databaseContext.Blocklist.Add(blocklist);
                        await _databaseContext.SaveChangesAsync();

                        _logger.LogInformation("User {Id_User} successfully blocked by user {UserId}", Id_User, user.Id);
                    }
                    else
                    {
                        _logger.LogWarning("User {Id_User} not found", Id_User);
                    }
                }
                else
                {
                    _logger.LogInformation("User {Id_User} is already blocked by user {UserId}", Id_User, user.Id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while blocking user {Id_User} by user {UserId}", Id_User, user.Id);
                throw;
            }
        }

        public async Task UnblockUser(int Id, UserModel user)
        {
            try
            {
                _logger.LogInformation("Attempting to unblock user {BlocklistId} by user {UserId}", Id, user.Id);

                var blocklist = await _databaseContext.Blocklist.FindAsync(Id);

                if (blocklist != null && blocklist.Id_User == user.Id)
                {
                    _databaseContext.Blocklist.Remove(blocklist);
                    await _databaseContext.SaveChangesAsync();

                    _logger.LogInformation("User {BlocklistId} successfully unblocked by user {UserId}", Id, user.Id);
                }
                else
                {
                    _logger.LogWarning("Blocklist entry {BlocklistId} not found or not owned by user {UserId}", Id, user.Id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while unblocking user {BlocklistId} by user {UserId}", Id, user.Id);
                throw;
            }
        }

        public async Task<List<Blocklist>> GetBlockUsers(UserModel user)
        {
            try
            {
                _logger.LogInformation("Retrieving blocked users for user {UserId}", user.Id);

                var blockedUsers = await _databaseContext.Blocklist
                    .Where(p => p.Id_User == user.Id)
                    .ToListAsync();

                _logger.LogInformation("Retrieved {Count} blocked users for user {UserId}", blockedUsers.Count, user.Id);

                return blockedUsers;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving blocked users for user {UserId}", user.Id);
                throw;
            }
        }
    }
}
