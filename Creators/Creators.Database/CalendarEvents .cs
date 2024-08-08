using System.ComponentModel.DataAnnotations;

namespace Creators.Creators.Database
{
    public class CalendarEvents
    {
        [Key]
        public int Id { get; set; }
        public string Id_Calendar { get; set; }
        public DateOnly DateOnly { get; set; }
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
        public string Description { get; set; }

        CreatorPage CreatorPage { get; set; }
    }
}
  