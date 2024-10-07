using System.ComponentModel.DataAnnotations;

namespace Creators.Creators.Database
{
    public class Messages
    {
        [Key]
        public int Id { get; set; }
        public int Chat_Id { get; set; }
        public string Id_Sender { get; set; }
        public DateTime DateTime { get; set; }
        public string Text { get; set; }

        public Chats Chat { get; set; }
    }
}
