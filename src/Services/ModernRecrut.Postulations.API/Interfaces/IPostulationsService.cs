using ModernRecrut.Postulations.API.Entities;
using ModernRecrut.Postulations.API.Entities.DTO;
namespace ModernRecrut.Postulations.API.Interfaces
{
    public interface IPostulationsService
    {
        public Task<IEnumerable<Postulation>> ObtenirPostulations();
        public Task<Postulation> ObtenirPostulationSelonId(Guid id);
        public Task<string> AjouterPostulation(RequetePostulation requetePostulation);
        public Task<string> ModifierPostulation(Postulation postulation);
        public Task<string> SupprimerPostulation(Guid id);
    }
}
