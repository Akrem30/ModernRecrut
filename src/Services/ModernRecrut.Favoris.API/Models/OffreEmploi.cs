namespace ModernRecrut.Favoris.API.Models
{
    public class OffreEmploi
    {
        public Guid Id { get; set; }
        public DateTime DateAffichage { get; set; }
        public DateTime DateFin { get; set; }
        public string? Description { get; set; }
        public string? Poste { get; set; }
    }
}
