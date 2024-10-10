namespace Creators.Creators.Models
{
    public class CreatorPageShow
    {
        public string Id_Creator { get; set; }
        public string AccountNumber { get; set; }
        public string ProfilName { get; set; }
        public string Description { get; set; }
        public byte[] ProfilPicture { get; set; }
        public string ProfilPictureExtension { get; set; }
        public string BioLinks { get; set; }
        public bool NotifyEvents { get; set; }
        public bool NotifyImages { get; set; }
        public string Id_Calendar { get; set; }
        public string Id_Donates { get; set; }
        public string Id_Photos { get; set; }

        public CreatorPageShow(string accountNumber, string description, byte[] profilPicture, string profilPictureExtension, string bioLinks, bool notifyEvents, bool notifyImages, string id_Creator, string id_Donates, string id_Photos)
        {
            AccountNumber = accountNumber;
            Description = description;
            ProfilPicture = profilPicture;
            ProfilPictureExtension = profilPictureExtension;
            BioLinks = bioLinks;
            NotifyEvents = notifyEvents;
            NotifyImages = notifyImages;
            Id_Creator = id_Creator;
            Id_Donates = id_Donates;
            Id_Photos = id_Photos;
        }

        public CreatorPageShow(string profilName, string description, byte[] profilPicture, string profilPictureExtension, string bioLinks, string id_Creator, string id_Calendar, string id_Donates, string id_Photos)
        {
            ProfilName = profilName;
            Description = description;
            ProfilPicture = profilPicture;
            ProfilPictureExtension = profilPictureExtension;
            BioLinks = bioLinks;    
            Id_Creator = id_Creator;
            Id_Calendar = id_Calendar;
            Id_Donates = id_Donates;
            Id_Photos = id_Photos;
        }

    }
}
