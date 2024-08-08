using System.ComponentModel.DataAnnotations;

namespace Creators.Creators.Database
{
    public class PhotoComments
    {
        [Key]
        public int Id { get; set; }
        public int Id_User { get; set; }
        public string CommentsGroup { get; set; }
        public DateOnly Date {  get; set; }
        public TimeOnly Time { get; set; }
        public bool Hidden { get; set; }
        public string Text { get; set; }

        public CreatorPhoto CreatorPhoto { get; set; }
    }
}
