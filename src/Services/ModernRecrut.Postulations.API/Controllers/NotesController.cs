using Microsoft.AspNetCore.Mvc;
using ModernRecrut.Postulations.API.Entities;
using ModernRecrut.Postulations.API.Entities.DTO;
using ModernRecrut.Postulations.API.Interfaces;
using ModernRecrut.Postulations.API.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ModernRecrut.Postulations.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INotesService _notesService;
        public NotesController(INotesService notesService)
        {
            _notesService = notesService;
        }

        /// <summary>
        /// Obtenir la liste des notes
        /// </summary>
        /// <response code="200">Liste des notes obtenues avec succès</response>
        /// <response code="404">Liste des notes introuvable</response> 
        /// <response code="500">Oops! Le service est indisponible pour le moment</response>
        // GET: api/<NotesController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var notes = await _notesService.ObtenirNotes();
            if (notes == null)
                return NotFound();

            return Ok(notes);
        }

        /// <summary>
        /// Obtenir une postulation spécifique par son identifiant
        /// </summary>
        /// <param name="id">L'identifiant de la note que vous souhaitez obtenir</param>
        /// <response code="200">note obtenue avec succès</response>
        /// <response code="404">note introuvable pour l'identifiant spécifié</response>    
        /// <response code="500">Oops! Le service est indisponible pour le moment</response>
        // GET api/<NtesController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var note = await _notesService.ObtenirNoteSelonId(id);
            if (note == null)
                return NotFound();

            return Ok(note);
        }

        /// <summary>
        /// Ajouter une note à la base de données 
        /// </summary>
        /// <param name="requeteNote">La nouvelle note à ajouter</param>
        /// <response code="201">note ajoutée avec succès</response>
        /// <response code="400">note ajoutée non valide</response>
        /// <response code="500">Oops! Le service est indisponible pour le moment</response>
        // POST api/<NotesController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RequeteNote requeteNote)
        {
            if (ModelState.IsValid)
            {
                await _notesService.AjouterNote(requeteNote);
            }
            return NoContent();
        }

        /// <summary>
        /// Modifer une note dans la base de données 
        /// </summary>
        /// <param name="id">L'identifiant de la note que vous souhaitez modifier</param>
        /// <response code="204">note modifiée avec succès</response>
        /// <response code="400">note modifiée non valide</response>
        /// <response code = "500" >Oops! Le service est indisponible pour le moment</response>
        // PUT api/<NotesController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] Note note)
        {

            await _notesService.ModifierNote(note);
            return NoContent();
        }

        /// <summary>
        /// Supprimer une note
        /// </summary>
        /// <param name="id">L'identifiant de la note que vous souhaitez supprimer</param>
        /// <response code="204">Suppression de la note effectuée avec succès </response>
        /// <response code="400">Erreur lors de la suppression</response> 
        /// <response code="500">Oops! Le service est indisponible pour le moment</response>
        // DELETE api/<NotesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _notesService.SupprimerNote(id);
            return NoContent();
        }
    }
}
