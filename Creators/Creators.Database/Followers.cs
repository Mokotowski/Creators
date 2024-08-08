using Microsoft.EntityFrameworkCore;

namespace Creators.Creators.Database
{
    public class Followers
    {
        public string Id_Creator { get; set; }
        public string Id_User { get; set; }
        public DateTime Since { get; set; }
    }
}
 