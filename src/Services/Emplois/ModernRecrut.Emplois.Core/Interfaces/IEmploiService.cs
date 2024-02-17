using ModernRecrut.Emplois.Core.DTO;
using ModernRecrut.Emplois.Core.Entites;

namespace ModernRecrut.Emplois.Core.Interfaces
{
    public interface IEmploiService
    {
        public Task Ajouter(RequeteOffreEmploi item);
        public Task<Emploi> ObtenirSelonId(Guid id);
        public Task<IEnumerable<Emploi>> ObtenirTout();
        public Task Modifier(Emploi item);
        public Task Supprimer(Emploi item);
    }
}
