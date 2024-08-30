namespace Creators.Creators.Models
{
    public class CreatorsSearch
    {
        public string Id_Creator { get; set; }
        public string ProfileName { get; set; }
        public string ProfilPicture { get; set; }

        public CreatorsSearch(string id_creator, string profileName, string profilePiture) 
        {
            Id_Creator = id_creator;
            ProfileName = profileName;
            ProfilPicture = profilePiture;
        }
    }
}
