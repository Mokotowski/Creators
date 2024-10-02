using Creators.Creators.Database;
using Creators.Creators.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Creators.Creators.Services
{
    public class ScheduleServices : IEventsFunctions, IScheduleData
    {
        private readonly DatabaseContext _databaseContext;
        private readonly ILogger<ScheduleServices> _logger;
        private readonly INotificationEmail _notificationEmail;
        private readonly IGetFollowers _getFollowers;

        public ScheduleServices(DatabaseContext databaseContext, ILogger<ScheduleServices> logger, INotificationEmail notificationEmail, IGetFollowers getFollowers)
        {
            _databaseContext = databaseContext;
            _logger = logger;
            _notificationEmail = notificationEmail;
            _getFollowers = getFollowers;   
        }

        public async Task<List<CalendarEvents>> ShowSchedule(string Id_Calendar)
        {
            try
            {
                if (string.IsNullOrEmpty(Id_Calendar))
                {
                    _logger.LogWarning("Invalid Id_Calendar: cannot be null or empty.");
                    throw new ArgumentException("Id_Calendar cannot be null or empty");
                }

                DateTime startDate = DateTime.Now.AddDays(-7);
                DateTime endDate = DateTime.Now.AddMonths(2);

                var events = _databaseContext.CalendarEvents
                    .Where(e => e.Id_Calendar == Id_Calendar && e.DateOnly >= DateOnly.FromDateTime(startDate) && e.DateOnly <= DateOnly.FromDateTime(endDate))
                    .OrderBy(e => e.DateOnly)
                    .ToList();

                _logger.LogInformation($"Fetched {events.Count} events for calendar {Id_Calendar}.");
                return events;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching schedule for calendar {Id_Calendar}");
                throw;
            }
        }

        public async Task<string> AddEvent(UserModel user, DateOnly date, TimeSpan start, TimeSpan end, string description)
        {
            try
            {
                if (date < DateOnly.FromDateTime(DateTime.Now))
                {
                    _logger.LogWarning("Attempt to add an event in the past: Date {EventDate}.", date);
                    return "Error: You can only add events for today or future dates.";
                }

                if (start >= end)
                {
                    _logger.LogWarning("Invalid event times: Start time {StartTime} is greater than or equal to end time {EndTime}.", start, end);
                    return "Invalid event times: Start time cannot be greater than or equal to end time.";
                }

                if (user.IsCreator)
                {
                    CreatorPage page = _databaseContext.CreatorPage.Find(user.Id);
                    string CreatorNick = user.UserName;
                    if (page != null)
                    {
                        CalendarEvents events = new CalendarEvents
                        {
                            Id_Calendar = page.Id_Calendar,
                            DateOnly = date,
                            Start = start,
                            End = end,
                            Description = description,
                            CreatorPage = page
                        };

                        _databaseContext.CalendarEvents.Add(events);
                        await _databaseContext.SaveChangesAsync();

                        PageData pageData = _databaseContext.PageData.Find(user.Id);

                        if (pageData != null && pageData.EmailNotificationsEvents)
                        {
                            List<Followers> followers = await _getFollowers.GetCreatorFollowers(user.Id);

                            foreach (Followers follower in followers)
                            {
                                UserModel userFollower = _databaseContext.Users.Single(p => p.Id == follower.Id_User);

                                if (userFollower != null)
                                {
                                    try
                                    {
                                        await _notificationEmail.SendNotificationAddCalendarEmail(
                                            userFollower.Email,
                                            page.Id_Calendar,
                                            date,
                                            start,
                                            end,
                                            description,
                                            CreatorNick
                                        );

                                        _logger.LogInformation("Notification email sent to {UserEmail} for event on {EventDate}.", userFollower.Email, date);
                                    }
                                    catch (Exception ex)
                                    {
                                        _logger.LogError(ex, "Error sending notification email to {UserEmail}.", userFollower.Email);
                                    }
                                }
                                else
                                {
                                    _logger.LogWarning("User not found for follower with ID {FollowerId}.", follower.Id_User);
                                }
                            }
                        }

                        _logger.LogInformation("Event added for calendar {CalendarId} by user {UserId}.", page.Id_Calendar, user.Id);
                        return "success";
                    }

                    _logger.LogWarning("Failed to add event: Calendar not found for user {UserId}.", user.Id);
                    return "error";
                }

                _logger.LogWarning("User {UserId} is not authorized to add events.", user.Id);
                return "error";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding event for user {UserId}.", user.Id);
                return "error";
            }
        }


        public async Task<string> DeleteEvent(UserModel user, int Id)
        {
            try
            {
                if (user.IsCreator)
                {
                    CalendarEvents events = _databaseContext.CalendarEvents.Find(Id);
                    CreatorPage page = _databaseContext.CreatorPage.Find(user.Id);

                    if (events != null && events.Id_Calendar == page.Id_Calendar)
                    {
                        if (events.DateOnly < DateOnly.FromDateTime(DateTime.Now))
                        {
                            _logger.LogWarning($"Attempt to delete a past event: Event ID {Id}.");
                            return "Error: You can only delete future events.";
                        }

                        _databaseContext.CalendarEvents.Remove(events);
                        _databaseContext.SaveChanges();

                        _logger.LogInformation($"Event {Id} deleted from calendar {page.Id_Calendar} by user {user.Id}.");
                        return "success";
                    }

                    _logger.LogWarning($"Failed to delete event {Id}: Event not found or user {user.Id} is not authorized.");
                    return "error";
                }

                _logger.LogWarning($"User {user.Id} is not authorized to delete events.");
                return "error";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting event {Id} for user {user.Id}");
                return "error";
            }
        }

        public async Task<string> UpdateEvent(UserModel user, int Id, DateOnly date, TimeSpan start, TimeSpan end, string description)
        {
            try
            {
                if (date < DateOnly.FromDateTime(DateTime.Now))
                {
                    _logger.LogWarning($"Attempt to update an event in the past: Date {date}.");
                    return "Error: You can only update events for today or future dates.";
                }

                if (start >= end)
                {
                    _logger.LogWarning($"Invalid event times: Start time {start} is greater than or equal to end time {end}.");
                    return "Invalid event times: Start time cannot be greater than or equal to end time.";
                }

                if (user.IsCreator)
                {
                    CalendarEvents events = _databaseContext.CalendarEvents.Find(Id);
                    CreatorPage page = _databaseContext.CreatorPage.Find(user.Id);

                    if (events != null && events.Id_Calendar == page.Id_Calendar)
                    {
                        events.DateOnly = date;
                        events.Start = start;
                        events.End = end;
                        events.Description = description;
                        _databaseContext.SaveChanges();

                        _logger.LogInformation($"Event {Id} updated for calendar {page.Id_Calendar} by user {user.Id}.");
                        return "success";
                    }

                    _logger.LogWarning($"Failed to update event {Id}: Event not found or user {user.Id} is not authorized.");
                    return "error";
                }

                _logger.LogWarning($"User {user.Id} is not authorized to update events.");
                return "error";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating event {Id} for user {user.Id}");
                return "error";
            }
        }


        public async Task<bool> IsCreator(string Id_Calendar, UserModel user)
        {
            try
            {
                _logger.LogInformation("Checking if user {UserId} is the creator for calendar {Id_Calendar}", user.Id, Id_Calendar);

                CreatorPage page = await _databaseContext.CreatorPage
                                        .SingleOrDefaultAsync(p => p.Id_Calendar == Id_Calendar);

                if (page == null)
                {
                    _logger.LogWarning("No CreatorPage found for calendar {Id_Calendar}", Id_Calendar);
                    return false;
                }

                bool isCreator = page.Id_Creator == user.Id;
                _logger.LogInformation("User {UserId} is {IsCreator} the creator for calendar {Id_Calendar}", user.Id, isCreator ? "indeed" : "not", Id_Calendar);

                return isCreator;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while checking if user {UserId} is the creator for calendar {Id_Calendar}", user.Id, Id_Calendar);
                return false;
            }
        }



    }
}
