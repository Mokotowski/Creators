namespace Creators.Creators.Services.Interface
{
    public interface INotificationEmail
    {
        public Task SendNotificationAddCalendarEmail(string email, string Id_Calendar, DateOnly date, TimeSpan start, TimeSpan end, string description, string Nick);

        public Task SendNotificationAddPhotoEmail(string email, string Id_Photos, DateOnly date, string description, string Nick);

    }
}
