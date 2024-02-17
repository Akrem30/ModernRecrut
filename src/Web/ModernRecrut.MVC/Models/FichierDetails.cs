using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ModernRecrut.MVC.Models
{
    public class FichierDetails
    {
        public Guid ID { get; set; }
        public string? Nom { get; set; }
        public Type Type { get; set; }
    }

    public enum Type
    {
        [Display(Name = "CV")]
        CV = 1,
        [Display(Name = "Lettre de motivation")]
        LETTREDEMOTIVATION = 2,
        [Display(Name = "Diplôme")]
        DIPLÔME = 3
    }
}
