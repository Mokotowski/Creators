namespace Creators.Creators.Models
{
    public class CreatorPageShow
    {
        public string Id_Creator { get; set; }
        public string AccountNumber { get; set; }
        public string ProfilName { get; set; }
        public string Description { get; set; }
        public string ProfilPicture { get; set; }
        public string BioLinks { get; set; }
        public bool NotifyEvents { get; set; }
        public bool NotifyImages { get; set; }
        public string Id_Calendar { get; set; }
        public string Id_Donates { get; set; }

        public CreatorPageShow(string accountNumber, string description, string profilPicture, string bioLinks, bool notifyEvents, bool notifyImages, string id_Creator, string id_Donates)
        {
            AccountNumber = accountNumber;
            Description = description;
            ProfilPicture = profilPicture;
            BioLinks = bioLinks;
            NotifyEvents = notifyEvents;
            NotifyImages = notifyImages;
            Id_Creator = id_Creator;
            Id_Donates = id_Donates;
        }

        public CreatorPageShow(string profilName, string description, string profilPicture, string bioLinks, string id_Creator, string id_Calendar, string id_Donates)
        {
            ProfilName = profilName;
            Description = description;
            ProfilPicture = profilPicture;
            BioLinks = bioLinks;    
            Id_Creator = id_Creator;
            Id_Calendar = id_Calendar;
            Id_Donates = id_Donates;
        }

    }
}
