using Creators.Creators.Database;
using Creators.Creators.Services.Interface;
using Microsoft.Extensions.Logging;

namespace Creators.Creators.Services
{
    public class FollowersServices : IFollow, IGetFollowers
    {
        private readonly DatabaseContext _databaseContext;
        private readonly ILogger<FollowersServices> _logger;

        public FollowersServices(DatabaseContext databaseContext, ILogger<FollowersServices> logger)
        {
            _databaseContext = databaseContext;
            _logger = logger;
        }

        public async Task<List<Followers>> GetCreatorFollowers(string Id_Creator)
        {
            try
            {
                if (string.IsNullOrEmpty(Id_Creator))
                {
                    _logger.LogWarning("GetCreatorFollowers: Id_Creator is null or empty");
                    throw new ArgumentException("Id_Creator cannot be null or empty");
                }

                _logger.LogInformation($"Fetching followers for creator with Id: {Id_Creator}");
                return _databaseContext.Followers.Where(p => p.Id_Creator == Id_Creator).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting followers for creator {Id_Creator}");
                throw;
            }
        }

        public async Task<List<Followers>> GetUserFollowing(UserModel user)
        {
            try
            {
                if (user == null || string.IsNullOrEmpty(user.Id))
                {
                    _logger.LogWarning("GetUserFollowing: user or user.Id is null or empty");
                    throw new ArgumentException("User or user.Id cannot be null or empty");
                }

                _logger.LogInformation($"Fetching users followed by user with Id: {user.Id}");
                return _databaseContext.Followers.Where(p => p.Id_User == user.Id).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting followings for user {user.Id}");
                throw;
            }
        }

        public async Task Follow(string Id_Creator, UserModel user)
        {
            try
            {
                if (string.IsNullOrEmpty(Id_Creator) || user == null || string.IsNullOrEmpty(user.Id))
                {
                    _logger.LogWarning("Follow: Invalid Id_Creator or user");
                    throw new ArgumentException("Invalid Id_Creator or user");
                }

                _logger.LogInformation($"User {user.Id} is trying to follow creator {Id_Creator}");
                UserModel creator = _databaseContext.Users.Find(Id_Creator);

                if (creator == null)
                {
                    _logger.LogWarning($"Follow: Creator with Id {Id_Creator} not found");
                    throw new Exception("Creator not found");
                }

                Followers follower = new Followers
                {
                    CreatorName = creator.UserName,
                    ProfileName = user.UserName,
                    Id_Creator = Id_Creator,
                    Id_User = user.Id,
                    Since = DateTime.Now,
                    UserModel = user
                };
                _databaseContext.Followers.Add(follower);
                _databaseContext.SaveChanges();
                _logger.LogInformation($"User {user.Id} successfully followed creator {Id_Creator}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while user {user.Id} was trying to follow creator {Id_Creator}");
                throw;
            }
        }

        public async Task UnFollow(string Id_Creator, UserModel user)
        {
            try
            {
                if (string.IsNullOrEmpty(Id_Creator) || user == null || string.IsNullOrEmpty(user.Id))
                {
                    _logger.LogWarning("UnFollow: Invalid Id_Creator or user");
                    throw new ArgumentException("Invalid Id_Creator or user");
                }

                _logger.LogInformation($"User {user.Id} is trying to unfollow creator {Id_Creator}");
                Followers follower = _databaseContext.Followers.SingleOrDefault(p => p.Id_Creator == Id_Creator && p.Id_User == user.Id);

                if (follower == null)
                {
                    _logger.LogWarning($"UnFollow: Follower not found for user {user.Id} and creator {Id_Creator}");
                    throw new Exception("Follower not found");
                }

                _databaseContext.Followers.Remove(follower);
                _databaseContext.SaveChanges();
                _logger.LogInformation($"User {user.Id} successfully unfollowed creator {Id_Creator}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while user {user.Id} was trying to unfollow creator {Id_Creator}");
                throw;
            }
        }

        public async Task<bool> IsFollowing(string Id_Creator, UserModel user)
        {
            try
            {
                if (string.IsNullOrEmpty(Id_Creator) || user == null || string.IsNullOrEmpty(user.Id))
                {
                    _logger.LogWarning("IsFollowing: Invalid Id_Creator or user");
                    throw new ArgumentException("Invalid Id_Creator or user");
                }

                _logger.LogInformation($"Checking if user {user.Id} is following creator {Id_Creator}");
                return _databaseContext.Followers.Any(p => p.Id_Creator == Id_Creator && p.Id_User == user.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while checking if user {user.Id} is following creator {Id_Creator}");
                throw;
            }
        }
    }
}
