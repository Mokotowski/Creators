using Creators.Creators.Database;
using Creators.Creators.Models;

namespace Creators.Creators.Services.Interface
{
    public interface IPhotoDataGet
    {
        public Task<List<PhotoHearts>> GetLikesCreator(int Id, UserModel user);
        public Task<List<PhotoComments>> GetCommentsCreator(int Id, UserModel user);

        public Task<List<HeartsForUser>> GetLikesUser(int Id);
        public Task<List<CommentsForUser>> GetCommentsUser(int Id, UserModel user);

        public Task<List<PhotoForCreator>> GetPhotosCreator(UserModel user);  //zwraca struktura zdjęce serca komentarze    zdjęcia dla creatora do wgłądu w lajki komen=tazre ukrywanie 
        public Task<List<PhotoForUser>> GetCreatorsPhotosUser(UserModel user);  //zwraca struktura zdjęce, ile serc, czy dał serce, komentarze max 5 na zdjęcie,          zdjecia wszystkich followancyh creatorów
        public Task<List<PhotoForUser>> GetCreatorPhotosUser(string Id_Photos, UserModel user);   //zwraca struktura zdjęce, ile serc, czy dał serce, komentarze max 5 na zdjęcie,        zjdęcia jednego twórcy

    }
}
