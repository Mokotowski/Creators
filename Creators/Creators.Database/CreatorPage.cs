using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Creators.Creators.Database
{
    public class CreatorPage
    {
        [Key]
        public string Id_Creator { get; set; }
        public string Id_Calendar { get; set; }
        public string Id_Photos { get; set;}
        public string Id_Donates { get; set;}
        public string Account_Numer {  get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Account_Balance { get; set; }
        [Column(TypeName = "decimal(5, 2)")]

        public decimal Site_Commission { get; set; }


        public UserModel User { get; set; }
        public PageData PageData { get; set; }
        public CreatorBalance CreatorBalance { get; set; }  
        public List<Donates> Donates { get; set; }
        public List<CalendarEvents> CalendarEvents { get; set; }
        public List<CreatorPhoto> CreatorPhotos { get; set; }
    }
}
