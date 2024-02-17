namespace ModernRecrut.Postulations.API.Interfaces
{
    public interface IDocumentsService
    {
        public Task<List<string>> ObtenirDocumentsSelonId(Guid utilisateurID);
    }
}
