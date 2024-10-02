namespace Creators.Creators.Models
{
    public class CommentsForUser
    {
        public int Id { get; set; }
        public string Id_User { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }
        public string Text { get; set; }
        public string User {  get; set; }
    }
}
