namespace ModernRecrut.Postulations.API.Entities
{
    public class Postulation :Entity
    {
        public decimal PretentionSalariale { get; set; }
        public DateTime DateDisponibilite { get; set; }
        public Guid CandidatID {  get; set; }
        public Guid OffreEmploiID { get; set; }

        public ICollection<Note>? Notes { get; set; }
    }
}
