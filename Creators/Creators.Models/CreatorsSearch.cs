namespace Creators.Creators.Models
{
    public class CreatorsSearch
    {
        public string Id_Creator { get; set; }
        public string ProfileName { get; set; }
        public byte[] ProfilPicture { get; set; }
        public string ProfilPictureExtension { get; set; }

        public CreatorsSearch(string id_creator, string profileName, byte[] profilePiture, string profilPictureExtension) 
        {
            Id_Creator = id_creator;
            ProfileName = profileName;
            ProfilPicture = profilePiture;
            ProfilPictureExtension = profilPictureExtension;
        }
    }
}
