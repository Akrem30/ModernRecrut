using Microsoft.AspNetCore.Hosting;
using Microsoft.VisualBasic.FileIO;
using ModernRecrut.Documents.API.Entities;
using Microsoft.AspNetCore.Hosting;
using ModernRecrut.Documents.API.Interfaces;
using ModernRecrut.Documents.API.Entities.DTO;

namespace ModernRecrut.Documents.API.Services
{
    public class FichierService :IFichierService
    {
        private readonly string _repertoire;
        private string _cheminFichiers;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private string _extension;
        private string _erreur;
        private readonly IConfiguration _config;
        private readonly long _tailleLimite;
        private readonly string[] _extensions;
        
        public FichierService(IWebHostEnvironment webHostEnvironment, IConfiguration config)
        {
            _webHostEnvironment = webHostEnvironment;
            _config = config;
            _repertoire = _config.GetValue<string>("Repertoire");
            _cheminFichiers = Path.Combine(_webHostEnvironment.ContentRootPath, _repertoire);
            _tailleLimite = _config.GetValue<long>("TailleLimite");
            _extensions = new string[] { ".pdf", ".docx", ".doc" };
 
        }
        public async Task<string> EnregistrerDocument(RequeteFichier fichier )
        {

                if(fichier==null || fichier.FileDetails==null || fichier.UtilisateurID == null || fichier.TypeDocument == null)
                {
                    _erreur = "erreur lors du téléchargmeent du document";
                     return _erreur;
                }
                if(ObtenirDocumentsSelonId(fichier.UtilisateurID).Count == 10)
                {
                     _erreur = "Vous avez atteint la limite des documents à télécharger, veuillez supprimer des documents existants pour pouvoir télécharger de nouveaux documents";
                     return _erreur;
                }
                _extension= Path.GetExtension(fichier.FileDetails.FileName);
                

                if(!_extensions.Any(e=>e==_extension))
                {
                    _erreur = "Vous pouvez déposer juste des documents word ou pdf";
                    return _erreur;
                }
                if(fichier.FileDetails.Length > _tailleLimite)
                {
                    _erreur = "Fichier volumineux";
                    return _erreur;
                }
                var fileDetails = new FichierDetails()
                {
                    
                    Nom = GenererNomFichier(fichier.UtilisateurID, fichier.TypeDocument, _extension),
                    Type = fichier.TypeDocument
                };


                if (!Directory.Exists(_cheminFichiers)) 
                   Directory.CreateDirectory(_cheminFichiers);
                

                var cheminFichier = Path.Combine(_cheminFichiers, fileDetails.Nom);
               
                using (var stream = new FileStream(cheminFichier, FileMode.Create))
                {   
                     
                        await fichier.FileDetails.CopyToAsync(stream);               
                }
                

                return _erreur;

           
        }
        public List<string> ObtenirDocumentsSelonId(Guid utilisateurID)
        {
         
            if (!Directory.Exists(_cheminFichiers))
                return new List<string>();

            
            var documentsUtilisateur = Directory.GetFiles(_cheminFichiers)
                .Select(Path.GetFileName)
                .Where(nom => nom.StartsWith(utilisateurID.ToString())).ToList();
            if(documentsUtilisateur==null || !documentsUtilisateur.Any())    
                return new List<string>();

            var documentsAvecType = new List<string>();
            foreach( var document in documentsUtilisateur)
            {
                documentsAvecType.Add(document + " : " + document.Split("_")[1]);
            }

            return documentsAvecType;
        }
        public  FileStream LireFichier(string nomFichier)
        {   
            var cheminFichier = Path.Combine(_cheminFichiers, nomFichier);
            if (File.Exists(cheminFichier))
            {
                var fichierStream = File.OpenRead(cheminFichier);

                return fichierStream;
            }

            return null;
        }
        public string SupprimerDocument(string nomFichier)
        {

            var cheminFichier = Path.Combine(_cheminFichiers, nomFichier);
            if (cheminFichier == null)
            {
                _erreur = "Le document n'existe pas";
                return _erreur;
            }
            if (File.Exists(cheminFichier))
            {
                File.Delete(cheminFichier);
                return _erreur;
            }

            _erreur = "Une erreur s'est produite lors de la suppression";
            return _erreur;
        }
        private string GenererNomFichier(Guid id,Entities.Type type,string extension)
        {
            Random rn = new();
            string nom;
            do
            {
                nom = id + "_" + type + "_" + rn.Next(1, 1000000).ToString() + extension;
            } while (ValiderNomExiste(id,nom));
            
            return nom;
        }
        private bool ValiderNomExiste(Guid id, string nom)
        {
            bool nomExiste = false;
            var documents = ObtenirDocumentsSelonId(id);
            foreach (var document in documents)
            {
                if (document.Split(":")[0].Trim() == nom)
                {
                    nomExiste = true;
                    break;
                }
            }
            return nomExiste;
            
        }
    }
}
