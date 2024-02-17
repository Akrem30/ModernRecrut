using Microsoft.EntityFrameworkCore;
using ModernRecrut.Postulations.API.Data;
using ModernRecrut.Postulations.API.Entities;
using ModernRecrut.Postulations.API.Entities.DTO;
using ModernRecrut.Postulations.API.Interfaces;

namespace ModernRecrut.Postulations.API.Services
{
    public class PostulationsService : IPostulationsService
    {
        private readonly IAsyncRepository<Postulation> _postulationRepository;
        private readonly IGenerationEvaluationService _evaluationService;
        private readonly IDocumentsService _documentsService;
        private readonly INotesService _notesService;
        private string _erreur;

        public PostulationsService(IGenerationEvaluationService evaluationService, IAsyncRepository<Postulation> postulationRepository, INotesService notesService, IDocumentsService documentsService)
        {
            _evaluationService = evaluationService;
            _postulationRepository = postulationRepository;
            _notesService = notesService;
            _documentsService = documentsService;
        }

        public async Task<string> AjouterPostulation(RequetePostulation requetePostulation)
        {
            var documentsCandidat = await _documentsService.ObtenirDocumentsSelonId(requetePostulation.CandidatID);
            if (!documentsCandidat.Any(d => d.Contains("CV")))
            {
                _erreur = "Un CV est obligatoire pour postuler. Veuillez déposer au préalable un CV dans votre espace Documents";
                return _erreur;
            }


            if (!documentsCandidat.Any(d => d.Contains("LETTREDEMOTIVATION")))
            {
                _erreur = "Une lettre de motivation est obligatoire pour postuler. Veuillez déposer au préalable une lettre de motivation dans votre espace Documents";
                return _erreur;
            }

            var postulations = await ObtenirPostulations();

            if (postulations.Where(p => p.CandidatID == requetePostulation.CandidatID).Any(p => p.OffreEmploiID == requetePostulation.OffreEmploiID))
            {
                _erreur = "Vous avez déjà postulé à cette offre d'emploi";
                return _erreur;
            }

            if (requetePostulation.PretentionSalariale <= 0)
            {
                _erreur = "Veuillez saisir une valeur valide pour le salaire";
                return _erreur;
            }

            if (requetePostulation.PretentionSalariale > 150000)
            {
                _erreur = "Votre prétention salariale est au-delà de nos limites";
                return _erreur;
            }
            if (requetePostulation.DateDisponibilite <= DateTime.Now || requetePostulation.DateDisponibilite > DateTime.Now.AddDays(45))
            {
                _erreur = "La date de disponibilité doit être supérieure à la date du jour et inférieure  45 jours";
                return _erreur;
            }
            Postulation postulation = new Postulation
            {

                CandidatID = requetePostulation.CandidatID,
                OffreEmploiID = requetePostulation.OffreEmploiID,
                DateDisponibilite = requetePostulation.DateDisponibilite,
                PretentionSalariale = requetePostulation.PretentionSalariale
            };

            await _postulationRepository.AddAsync(postulation);
            var note = _evaluationService.GenererEvaluation(postulation.PretentionSalariale);
            note.PostulationID = postulation.Id;
            RequeteNote requeteNote = new RequeteNote
            {
                Description = note.Description,
                NomEmetteur = note.NomEmetteur,
                PostulationID = note.PostulationID
            };
            await _notesService.AjouterNote(requeteNote);
            return _erreur;

        }

        public async Task<string> ModifierPostulation(Postulation postulation)
        {
            var postulationAmodifier = await ObtenirPostulationSelonId(postulation.Id);
            if (postulationAmodifier.DateDisponibilite > DateTime.Now.AddDays(5) || postulationAmodifier.DateDisponibilite < DateTime.Now.AddDays(-5))
            {
                _erreur = "Vous ne pouvez pas modifier la date de disponibilité pour cette postulation";
                return _erreur;
            }
            if (postulation.PretentionSalariale <= 0)
            {
                _erreur = "Veuillez saisir une valeur valide pour le salaire";
                return _erreur;
            }
            if (postulation.PretentionSalariale > 150000)
            {
                _erreur = "Votre prétention salariale est au-delà de nos limites";
                return _erreur;
            }
            if (postulation.DateDisponibilite <= DateTime.Now || postulation.DateDisponibilite > DateTime.Now.AddDays(45))
            {
                _erreur = "La date de disponibilité doit être supérieure à la date du jour et inférieure  45 jours";
                return _erreur;
            }

            if (postulationAmodifier.PretentionSalariale != postulation.PretentionSalariale)
            {
                postulationAmodifier.PretentionSalariale = postulation.PretentionSalariale;

                var noteAmodifier = postulationAmodifier.Notes.FirstOrDefault(n => n.NomEmetteur == "ApplicationPostulation");
                noteAmodifier.Description = _evaluationService.GenererEvaluation(postulation.PretentionSalariale).Description;

                await _notesService.ModifierNote(noteAmodifier);
            }
            postulationAmodifier.DateDisponibilite = postulation.DateDisponibilite;
            await _postulationRepository.EditAsync(postulationAmodifier);
            return _erreur;
        }

        public async Task<IEnumerable<Postulation>> ObtenirPostulations()
        {
            var postulations = await _postulationRepository.ListAsync();
            var notes = await _notesService.ObtenirNotes();

            foreach ( var postulation in postulations) 
            {
                postulation.Notes = notes.Where(e => e.PostulationID == postulation.Id).ToList();
            }

            return postulations;
        }

        public async Task<Postulation> ObtenirPostulationSelonId(Guid id)
        {
            return await _postulationRepository.GetByIdAsync(id);
        }

        public async Task<string> SupprimerPostulation(Guid id)
        {
            var postulationAsupprimer = await ObtenirPostulationSelonId(id);
            if (postulationAsupprimer.DateDisponibilite > DateTime.Now.AddDays(5) || postulationAsupprimer.DateDisponibilite < DateTime.Now.AddDays(-5))
            {
                _erreur = "Vous ne pouvez pas supprimer cette postulation";
                return _erreur;
            }

            if (postulationAsupprimer != null)
            {
                await _postulationRepository.DeleteAsync(postulationAsupprimer);
                return _erreur;
            }
            _erreur = "La postulation n'existe pas";
            return _erreur;
        }
    }
}
