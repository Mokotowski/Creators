using System.ComponentModel.DataAnnotations;

namespace Creators.Creators.Database
{
    public class Chats
    {
        [Key]
        public int Id { get; set; }
        public string Id_User1 { get; set; }
        public string Id_User2 { get; set; }
        public string User1 { get; set; }
        public string User2 { get; set; }

        public List<Messages> Messages { get; set; }
    }
}
