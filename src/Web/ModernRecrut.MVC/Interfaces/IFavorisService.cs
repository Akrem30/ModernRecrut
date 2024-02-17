using ModernRecrut.MVC.Models;
using ModernRecrut.MVC.Models.DTO;

namespace ModernRecrut.MVC.Interfaces
{
    public interface IFavorisService
    {
        public Task<Favoris> ObtenirTout(string ipAddress);

        public Task AjouterFavorite(RequeteAjoutFavorite requeteAjoutFavorite);

        public Task SupprimerFavorite(string ipAddress, Guid offreId);
    }
}
