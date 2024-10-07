using Creators.Creators.Database;
using Creators.Creators.Models;
using Creators.Creators.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Creators.Creators.Services
{
    public class UserDataServices : IUserData
    {
        private readonly DatabaseContext _databaseContext;
        private readonly ILogger<UserDataServices> _logger; 

        public UserDataServices(DatabaseContext databaseContext, ILogger<UserDataServices> logger)
        {
            _databaseContext = databaseContext;
            _logger = logger;
        }

        public async Task<List<UserForChats>> GetUsers(string User, UserModel user)
        {
            try
            {
                if (string.IsNullOrEmpty(User) || user == null)
                {
                    _logger.LogWarning("GetUsers: Invalid input - User or currentUser is null.");
                    return null;
                }

                _logger.LogInformation("GetUsers: Searching for users containing the string '{SearchTerm}'", User);

                var users = await _databaseContext.Users
                    .Where(p => p.UserName.ToLower().Contains(User.ToLower()) && p.UserName != user.UserName)
                    .ToListAsync();

                List<UserForChats> usersforchats = new List<UserForChats>();

                foreach (UserModel usermodel in users)
                {
                    bool Exist = _databaseContext.Chats.Any(p => p.Id_User1 == usermodel.Id && p.Id_User2 == user.Id || p.Id_User1 == user.Id && p.Id_User2 == usermodel.Id);
                    int Chat_Id = -1;


                    if (Exist)
                    {
                        Chat_Id = _databaseContext.Chats
                            .Where(p => (p.Id_User1 == usermodel.Id && p.Id_User2 == user.Id) ||
                                        (p.Id_User1 == user.Id && p.Id_User2 == usermodel.Id))
                            .Select(p => p.Id)
                            .SingleOrDefault();
                    }
                    else
                    {

                    }

                    UserForChats uchat = new UserForChats()
                    {
                        Id = usermodel.Id,
                        UserName = usermodel.UserName,
                        Firstname = usermodel.Firstname,
                        Lastname = usermodel.Lastname,
                        ChatExist = Exist,
                        Chat_Id = Chat_Id
                    };

                    usersforchats.Add(uchat);
                }


                if (users.Count == 0)
                {
                    _logger.LogInformation("GetUsers: No users found containing the string '{SearchTerm}'", User);
                }

                return usersforchats;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetUsers: Error occurred while searching for users containing '{SearchTerm}'", User);
                return null;
            }
        }



    }
}
