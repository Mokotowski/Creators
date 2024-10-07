using Creators.Creators.Database;
using Creators.Creators.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Creators.Creators.Services
{
    public class ChatServices : IChatsActions
    {
        private readonly DatabaseContext _databaseContext;
        private readonly ILogger<ChatServices> _logger;

        public ChatServices(DatabaseContext databaseContext, ILogger<ChatServices> logger)
        {
            _databaseContext = databaseContext;
            _logger = logger;
        }

        public async Task<List<Chats>> GetChats(UserModel user)
        {
            try
            {
                _logger.LogInformation("Fetching chats for user {UserId}", user.Id);
                return await _databaseContext.Chats
                    .Where(p => p.Id_User1 == user.Id || p.Id_User2 == user.Id)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching chats for user {UserId}", user.Id);
                throw;
            }
        }

        public async Task<Chats> GetChat(int Id)
        {
            try
            {
                _logger.LogInformation("Fetching chat {ChatId}", Id);
                var chat = await _databaseContext.Chats.FindAsync(Id);

                if (chat != null)
                {
                    chat.Messages = await _databaseContext.Messages
                        .Where(p => p.Chat_Id == chat.Id)
                        .ToListAsync();
                }
                return chat;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching chat {ChatId}", Id);
                throw;
            }
        }

        public async Task<Chats> AddMessage(int Id, UserModel user, string Text)
        {
            try
            {
                _logger.LogInformation("Adding message to chat {ChatId} by user {UserId}", Id, user.Id);

                if (Text == "Message deleted")
                {
                    return await _databaseContext.Chats.FindAsync(Id);
                }

                var message = new Messages
                {
                    Chat_Id = Id,
                    Id_Sender = user.Id,
                    DateTime = DateTime.Now,
                    Text = Text
                };

                await _databaseContext.Messages.AddAsync(message);
                await _databaseContext.SaveChangesAsync();

                return await _databaseContext.Chats.FindAsync(Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding message to chat {ChatId} by user {UserId}", Id, user.Id);
                throw;
            }
        }

        public async Task<Chats> DeleteMessage(int Id, UserModel user)
        {
            try
            {
                _logger.LogInformation("Deleting message {MessageId} by user {UserId}", Id, user.Id);

                var message = await _databaseContext.Messages.FindAsync(Id);
                if (message != null && message.Id_Sender == user.Id)
                {
                    message.Text = "Message deleted";
                    await _databaseContext.SaveChangesAsync();
                }
                return await _databaseContext.Chats.FindAsync(message.Chat_Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting message {MessageId} by user {UserId}", Id, user.Id);
                throw;
            }
        }

        public async Task<Chats> MakeChat(UserModel user_me, string user_friend)
        {
            try
            {
                _logger.LogInformation("Creating or fetching chat between user {UserId} and {FriendId}", user_me.Id, user_friend);

                if (user_me.Id == user_friend)
                {
                    _logger.LogWarning("User tried to create a chat with themselves.");
                    return new Chats();
                }

                var existingChat = await _databaseContext.Chats
                    .FirstOrDefaultAsync(p => (p.Id_User1 == user_me.Id && p.Id_User2 == user_friend) ||
                                              (p.Id_User1 == user_friend && p.Id_User2 == user_me.Id));

                if (existingChat != null)
                {
                    return existingChat;
                }

                var user = await _databaseContext.Users.FindAsync(user_friend);
                if (user == null)
                {
                    _logger.LogWarning("Friend user {FriendId} not found.", user_friend);
                    return new Chats();
                }

                var newChat = new Chats
                {
                    Id_User1 = user_me.Id,
                    Id_User2 = user_friend,
                    User1 = user_me.UserName,
                    User2 = user.UserName
                };

                await _databaseContext.Chats.AddAsync(newChat);
                await _databaseContext.SaveChangesAsync();

                return newChat;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating or fetching chat between user {UserId} and {FriendId}", user_me.Id, user_friend);
                throw;
            }
        }
    }
}
