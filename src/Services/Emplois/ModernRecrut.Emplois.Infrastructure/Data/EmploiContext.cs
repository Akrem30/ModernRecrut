using Microsoft.EntityFrameworkCore;
using ModernRecrut.Emplois.Core.Entites;

namespace ModernRecrut.Emplois.Infrastructure.Data
{
    public class EmploiContext : DbContext
    {
        public EmploiContext(DbContextOptions<EmploiContext> options) : base(options) { }

        public DbSet<Emploi> Emplois { get; set; }
    }
}
