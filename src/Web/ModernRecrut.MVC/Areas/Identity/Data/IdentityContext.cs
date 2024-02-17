using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ModernRecrut.MVC.Areas.Identity.Data;

namespace ModernRecrut.MVC.Data;

public class IdentityContext : IdentityDbContext<Utilisateur>
{
    public IdentityContext(DbContextOptions<IdentityContext> options)
       : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        string ADMIN_USER_ID = "5f159a43-e45f-4d3c-8760-c757d850d77d";
        string ADMIN_ROLE_ID = "0d88d65e-266e-463f-9eec-1d314a91baa2";

        base.OnModelCreating(builder);

        //Seed Roles
        builder.Entity<IdentityRole>().HasData(new IdentityRole
        {
            Id = ADMIN_ROLE_ID,
            Name = "Admin",
            NormalizedName = "ADMIN",
            ConcurrencyStamp = ADMIN_ROLE_ID,
        });

        builder.Entity<IdentityRole>().HasData(new IdentityRole
        {
            Id = Guid.NewGuid().ToString(),
            Name = "RH",
            NormalizedName = "RH",
            ConcurrencyStamp = Guid.NewGuid().ToString(),
        });

        builder.Entity<IdentityRole>().HasData(new IdentityRole
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Candidat",
            NormalizedName = "CANDIDAT",
            ConcurrencyStamp = Guid.NewGuid().ToString(),
        });

        builder.Entity<IdentityRole>().HasData(new IdentityRole
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Employé",
            NormalizedName = "EMPLOYÉ",
            ConcurrencyStamp = Guid.NewGuid().ToString(),
        });

        //Create admin user
        var appUser = new Utilisateur
        {
            Id = ADMIN_USER_ID,
            Email = "admin@gmail.com",
            EmailConfirmed = true,
            Prenom = "Admin",
            Nom = "Admin",
            UserName = "admin@gmail.com",
            NormalizedUserName = "ADMIN@GMAIL.COM"
        };

        //Set admin password
        PasswordHasher<Utilisateur> ph = new PasswordHasher<Utilisateur>();
        appUser.PasswordHash = ph.HashPassword(appUser, "Admin@123");

        //Seed admin user
        builder.Entity<Utilisateur>().HasData(appUser);


        //set admin role to user admin
        builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
        {
            RoleId = ADMIN_ROLE_ID,
            UserId = ADMIN_USER_ID
        });


        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
