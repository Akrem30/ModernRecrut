using Microsoft.AspNetCore.Mvc;
using ModernRecrut.Documents.API.Entities.DTO;
using ModernRecrut.Documents.API.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ModernRecrut.Documents.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly IFichierService _fichierService;
        private readonly ILogger<DocumentsController> _logger;
        
    
        public DocumentsController(IFichierService fichierService, ILogger<DocumentsController> logger) 
        {
            _fichierService = fichierService;
            _logger = logger;
         
        }

        /// <summary>
        /// Obtenir La liste de document d'un utilisateur
        /// </summary>
        /// <param name="utilisateurID">L'id de l'utilisateur dont on retourne la liste de ses documents</param>
        /// <response code="200">Liste de documentsobtenue avec succès</response>
        /// <response code="400">Erreur lors de la récupération des documents</response> 
        // GET api/<DocumentsController>/5
        [HttpGet("{utilisateurID}")]
        public IActionResult  Get(Guid utilisateurID)
        {
            var documents =_fichierService.ObtenirDocumentsSelonId(utilisateurID);
            if (documents == null)
            {
                _logger.LogError(400, $"Erreur avec le code 400 lors de l'obtention des documents de l'utilisateur avec l'id {utilisateurID}");
                return BadRequest("Erreur lors de la récupération de la liste des documents");

            }
            _logger.LogInformation(CustomLogEventsDocuments.Lecture, $"Lecture des documents de l'utilisateur avec l'id {utilisateurID}");
            return Ok(documents);
        }

        /// <summary>
        /// Lire le contenu d'un fichier
        /// </summary>
        /// <param name="nomFichier">Le nom du fichier dont on veut lire son contenu</param>
        /// <response code="200">contenu obtenu avec succès</response>
        /// <response code="400">Erreur lors de la lecture du contenu du fichier</response>

        [HttpGet("documents/{nomFichier}")]
        public IActionResult LireFichier(string nomFichier)
        {
            var fichierStream = _fichierService.LireFichier(nomFichier);
            if (fichierStream == null)
            {
                _logger.LogError(400, $"Erreur avec le code 400 lors de la lecture du contenu du fichier {nomFichier}");
                return BadRequest("Le fichier est introuvable");
            }

            _logger.LogInformation(CustomLogEventsDocuments.Lecture, $"Lecture du contenu du document {nomFichier} ");
            return nomFichier.EndsWith(".pdf") ? File(fichierStream, "application/pdf") :
                                                  File(fichierStream, "application/docx");
                      
        }
        /// <summary>
        /// Ajouter un document
        /// </summary>  
        /// <param name="fichier">Le nouveau document à ajouter</param>
        /// <response code="201">document ajouté  avec succès</response>
        /// <response code="400">erreur lors de l'ajout du document</response>
        // POST api/<DocumentsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] RequeteFichier fichier)
        {

            if (fichier == null)
            {
                _logger.LogError(400, $"Erreur avec le code 400 lors de l'ajout d'un fichier.");
                return BadRequest("Aucun fichier n'a été téléchargé.");
            }  

                var erreur = await _fichierService.EnregistrerDocument(fichier);

            if (String.IsNullOrEmpty(erreur))
            {
                _logger.LogInformation(CustomLogEventsDocuments.Creation, $"ajout du document de l'utilisateur avec l'id {fichier.UtilisateurID}");
                return Ok("Le fichier a été enregistré avec succès.");
            }
            else
            {
                _logger.LogError(400, $"Erreur avec le code 400 lors de l'ajout d'un fichier pour l'utilisateur avec id {fichier.UtilisateurID}.");
                return BadRequest(new { _erreur = erreur });
            }
            
        }
        /// <summary>
        /// Supprimer un document
        /// </summary>
        /// <param name="nomFichier">Le nom du fichier à supprimer</param>
        /// <response code="204">Suppression du document effectuée avec succès </response>
        /// <response code="400">Erreur lors de la suppression du document</response>
        /// <response code="404">Document introuvable</response> 
        // DELETE api/<DocumentsController>/5
        [HttpDelete("{nomFichier}")]
        public IActionResult Delete(string nomFichier)
        {
          

            var erreur =  _fichierService.SupprimerDocument(nomFichier);

            if (String.IsNullOrEmpty(erreur))
            {
                _logger.LogInformation(CustomLogEventsDocuments.Lecture, $"Lecture du document {nomFichier}");
                _logger.LogInformation(CustomLogEventsDocuments.Suppression, $"suppression du document {nomFichier}");
                return Ok("Le document a été supprimé avec succès.");
            }
            else
            {
                _logger.LogError(400, $"Erreur avec le code 400 lors de la suppresison du fichier.");
                return BadRequest(new { _erreur = erreur });
            }
        }
    }
}
