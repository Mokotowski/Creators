using Microsoft.AspNetCore.Mvc;

namespace Creators.Creators.Services.Interface
{
    public interface ILikes
    {
        public Task LikePhoto(string HeartGroup, string Id_User);
        public Task UnLinkePhoto(int Id);
    }
}
