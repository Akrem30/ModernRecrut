using ModernRecrut.MVC.Models.DTO;
namespace ModernRecrut.MVC.Interfaces
{
    public interface IDocumentsService
    {
        public Task EnregistrerDocument(RequeteFichier fichier);
        public Task <List<string>> ObtenirDocumentsSelonId(Guid utilisateurID);     
        public  Task<string> LireFichier(string nomFichier);
        public Task SupprimerDocument(string nomFichier);
    }
}
