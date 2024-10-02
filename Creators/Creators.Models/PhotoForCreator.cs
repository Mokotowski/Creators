using Creators.Creators.Database;

namespace Creators.Creators.Models
{
    public class PhotoForCreator
    {
        public int Id { get; set; }
        public string Id_Photos { get; set; }
        public string CommentsGroup { get; set; }
        public string HeartGroup { get; set; }
        public string Description { get; set; }
        public bool CommentsOpen { get; set; }
        public byte[] File { get; set; }
        public string FileExtension { get; set; }
        public DateTime DateTime { get; set; }

        public List<PhotoComments> comments { get; set; }
        public List<PhotoHearts> Hearts { get; set; }
    }
}
