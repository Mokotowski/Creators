using Creators.Creators.Database;

namespace Creators.Creators.Services.Interface
{
    public interface IScheduleData
    {
        public Task<bool> IsCreator(string Id_Calendar, UserModel user); 
    }
}
