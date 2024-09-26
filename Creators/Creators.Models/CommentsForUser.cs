namespace Creators.Creators.Models
{
    public class CommentsForUser
    {
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }
        public string Text { get; set; }
        public string User {  get; set; }
    }
}
