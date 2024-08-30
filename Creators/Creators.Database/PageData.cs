using System.ComponentModel.DataAnnotations;

namespace Creators.Creators.Database
{
    public class PageData
    {
        [Key]
        public string Id_Creator { get; set; }
        public string Description { get; set; }
        public string ProfilName { get; set; }
        public string ProfilPicture { get; set; }
        public bool EmailNotificationsPhoto { get; set; }
        public bool EmailNotificationsEvents { get; set; }

        public string BioLinks { get; set; } 
        public CreatorPage CreatorPage { get; set; }
    }
}
