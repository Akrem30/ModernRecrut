namespace ModernRecrut.Postulations.API.Entities
{
    public class Note : Entity
    {
        public string? Description { get; set; }
        public string? NomEmetteur { get; set; }
        public Guid PostulationID { get; set; }
    }
}
