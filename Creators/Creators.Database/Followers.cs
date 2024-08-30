using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Creators.Creators.Database
{
    public class Followers
    {
        [Key]
        public int Id { get; set; }
        public string CreatorName { get; set; }
        public string ProfileName { get; set; }
        public string Id_Creator { get; set; }
        public string Id_User { get; set; }
        public DateTime Since { get; set; }

        public UserModel UserModel { get; set; }

    }
}
 