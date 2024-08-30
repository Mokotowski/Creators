using System.ComponentModel.DataAnnotations;

namespace Creators.Creators.Database
{
    public class CreatorPhoto
    {
        [Key]
        public int Id { get; set; }
        public string Id_Photos { get; set; }
        public string CommentsGroup { get; set; }
        public string HeartGroup { get; set; }
        public string Description { get; set; }

        public DateTime DateTime { get; set; }

        public CreatorPage CreatorPage { get; set; }

        public List<PhotoComments> Comments { get; set; }
        public List<PhotoHearts> Hearts { get; set; }
    }
}
