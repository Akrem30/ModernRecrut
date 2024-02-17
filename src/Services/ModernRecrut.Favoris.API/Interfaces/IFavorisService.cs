using ModernRecrut.Favoris.API.Models.DTO;

namespace ModernRecrut.Favoris.API.Interfaces
{
    public interface IFavorisService
    {
        //Obtenir la liste de favoris d'un utilisateur selon son adresse ip
        public Task<Models.Favoris> ObtenirTout(string ipAddress);

        public Task<List<string>> Ajouter(RequeteFavoris requeteFavoris);

        public Task<List<string>> Supprimer(string ipdAddress, Guid offreId);
    }
}
