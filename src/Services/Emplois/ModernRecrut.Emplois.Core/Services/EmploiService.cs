using ModernRecrut.Emplois.Core.DTO;
using ModernRecrut.Emplois.Core.Entites;
using ModernRecrut.Emplois.Core.Interfaces;

namespace ModernRecrut.Emplois.Core.Services
{
    public class EmploiService : IEmploiService
    {
        private readonly IAsyncRepository<Emploi> _emploiRepository;

        public EmploiService(IAsyncRepository<Emploi> emploiRepository)
        {
            _emploiRepository = emploiRepository;
        }
        public async Task Ajouter(RequeteOffreEmploi requeteOffreEmploi)
        {
            Emploi emploi = new Emploi()
            {
                DateAffichage = requeteOffreEmploi.DateAffichage,
                DateFin = requeteOffreEmploi.DateFin,
                Poste = requeteOffreEmploi.Poste,
                Description = requeteOffreEmploi.Description
            };

            await _emploiRepository.AddAsync(emploi);
        }

        public async Task<Emploi> ObtenirSelonId(Guid id)
        {
            return await _emploiRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Emploi>> ObtenirTout()
        {
            return await _emploiRepository.ListAsync();
        }

        public async Task Modifier(Emploi emploi)
        {
            await _emploiRepository.EditAsync(emploi);
        }

        public async Task Supprimer(Emploi emploi)
        {
            await _emploiRepository.DeleteAsync(emploi);
        }
    }
}
