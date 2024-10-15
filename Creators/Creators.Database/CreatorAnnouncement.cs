using System.ComponentModel.DataAnnotations;

namespace Creators.Creators.Database
{
    public class CreatorAnnouncement
    {
        [Key]
        public int Id { get; set; }
        public string Creator { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Id_Announcement { get; set; }
        public bool IsUpdated { get; set; }
        public DateTime DateTime { get; set; }

        public CreatorPage CreatorPage { get; set; }
    }
}
