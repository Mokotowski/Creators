using Creators.Creators.Database;
using Microsoft.AspNetCore.Mvc;

namespace Creators.Creators.Services.Interface
{
    public interface IComments
    {
        public  Task AddComment(string Id_User, string CommentsGroup, string Text);
        public  Task DeleteComment(int Id, UserModel user);
    }
}
