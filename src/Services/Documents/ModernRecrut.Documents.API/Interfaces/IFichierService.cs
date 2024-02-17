using Microsoft.VisualBasic.FileIO;
using ModernRecrut.Documents.API.Entities.DTO;

namespace ModernRecrut.Documents.API.Interfaces
{
    public interface IFichierService
    {
        public Task<string> EnregistrerDocument(RequeteFichier fichier);

        public List<string> ObtenirDocumentsSelonId(Guid utilisateurID);
        public FileStream LireFichier(string nomFichier);
        public string SupprimerDocument(string nomFichier);
    }
}
