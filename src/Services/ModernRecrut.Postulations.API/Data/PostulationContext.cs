using Microsoft.EntityFrameworkCore;
using ModernRecrut.Postulations.API.Entities;

namespace ModernRecrut.Postulations.API.Data
{
    public class PostulationContext :DbContext
    {
        public PostulationContext(DbContextOptions<PostulationContext> options) : base(options)
        {

        }

        public DbSet<Postulation> Postulations { get; set; }
        public DbSet<Note> Notes { get; set; }
    }
}
