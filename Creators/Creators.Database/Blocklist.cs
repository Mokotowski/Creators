using System.ComponentModel.DataAnnotations;

namespace Creators.Creators.Database
{
    public class Blocklist
    {
        [Key]
        public int Id { get; set; }
        public string Id_User { get; set; }
        public string Id_BlockUser { get; set; }
        public string UserName { get; set; }
    }
}
