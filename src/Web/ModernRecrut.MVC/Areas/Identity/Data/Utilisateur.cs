using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ModernRecrut.MVC.Areas.Identity.Data;

// Add profile data for application users by adding properties to the Utilisateur class
    public class Utilisateur : IdentityUser
    {
    public string Prenom { get; set; }
    public string Nom { get; set; }

    [NotMapped]
    public string NomComplet => $"{Prenom} {Nom}";
    public TypeUtilisateur Type { get; set; }
    }
    public enum TypeUtilisateur
    {
        Employé=1,
        Candidat=2
    }


