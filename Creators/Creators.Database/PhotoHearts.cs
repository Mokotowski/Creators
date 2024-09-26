using System.ComponentModel.DataAnnotations;

namespace Creators.Creators.Database
{
    public class PhotoHearts
    {
        [Key]
        public int Id { get; set; }
        public string Id_User { get; set; }
        public string HeartGroup { get; set; }

        public CreatorPhoto CreatorPhoto { get; set; }
    }
}
