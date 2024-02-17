using ModernRecrut.Favoris.API.Models;
using ModernRecrut.Favoris.API.Models.DTO;

namespace ModernRecrut.Favoris.API.Interfaces
{
    public interface IFavorisServiceProxy
    {
        //public Task<OffreEmploi> ObtenirOffreEmploiAsync(Guid offreID);
        public Task<List<OffreEmploi>> ObtenirOffresEmploisValides();
    }
}
