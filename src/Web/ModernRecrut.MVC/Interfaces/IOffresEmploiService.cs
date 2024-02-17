using ModernRecrut.MVC.Models;
using ModernRecrut.MVC.Models.DTO;

namespace ModernRecrut.MVC.Interfaces
{
    public interface IOffresEmploiService
    {
        public Task AjouterOffreEmploi(RequeteAjoutOffreEmploi requeteAjoutOffreEmploi);

        public Task<OffreEmploi> ObtenirOffreEmploiSelonId(Guid id);

        public Task<IEnumerable<OffreEmploi>> ObtenirTousOffresEmploi();

        public Task<IEnumerable<OffreEmploi>> ObtenirOffresEmploiValides();

        public Task ModifierOffreEmploi(OffreEmploi offreEmploi);

        public Task SupprimerOffreEmploi(Guid id);
    }
}
