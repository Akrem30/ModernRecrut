using System.ComponentModel;

namespace ModernRecrut.MVC.Models
{
    public class Note
    {
        public Guid Id { get; set; }
        public string? Description { get; set; }
        [DisplayName("Nom émetteur")]
        public string? NomEmetteur { get; set; }
        public Guid PostulationID { get; set; }
    }
}
