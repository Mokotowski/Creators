using Creators.Creators.Database;
using Microsoft.AspNetCore.Mvc;

namespace Creators.Creators.Services.Interface
{
    public interface IEventsFunctions
    {
        public Task<List<CalendarEvents>> ShowSchedule(string Id_Calendar);
        public Task<string> AddEvent(UserModel user, DateOnly date, TimeSpan start, TimeSpan end, string description);
        public Task<string> DeleteEvent(UserModel user, int Id);
        public Task<string> UpdateEvent(UserModel user, int Id, DateOnly date, TimeSpan start, TimeSpan end, string description);
    }
}
