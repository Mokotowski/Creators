namespace Creators.Creators.Models
{
    public class PhotoForUser
    {
        //zwraca struktura zdjęce, ile serc, czy dał serce, komentarze max 5 na zdjęcie, 
        public int Id { get; set; }
        public string Description { get; set; }
        public bool CommentsOpen { get; set; }
        public byte[] File { get; set; }
        public string FileExtension { get; set; }
        public DateTime DateTime { get; set; }
        public string Creator { get; set; }

        public bool GiveLike { get; set; }
        public bool GiveComment { get; set; }
        public int CountLike { get; set; }

        public string CommentsGroup { get; set; }
        public string HeartGroup { get; set; }



        public List<CommentsForUser> CommentsForUsers { get; set; } // jeżeli id usera jest takie jak komentarza to wyświetlać ukryty komentarz
    }
}
