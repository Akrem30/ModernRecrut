using ModernRecrut.MVC.Models;
using ModernRecrut.MVC.Models.DTO;
namespace ModernRecrut.MVC.Interfaces
{
    public interface IPostulationsService
    {
        public Task<List<Postulation>> ObtenirPostulations();
        //  public Task<List<Postulation>> ObtenirPostulationsSelonCandidat(Guid candidatID);
        public Task<Postulation> ObtenirPostulationSelonId(Guid id);
        public Task AjouterPostulation(RequetePostulation requetePostulation);
        public Task ModifierPostulation(Postulation postulation);
        public Task SupprimerPostulation(Guid id);
    }
}
