using ModernRecrut.Favoris.API.Models.DTO;

namespace ModernRecrut.Favoris.API.Models
{
    public class Favoris
    {
        public string Cle { get; set; }
        public List<OffreEmploi>? Contenu { get; set; } = new List<OffreEmploi>();
    }
}
