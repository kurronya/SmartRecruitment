using Microsoft.EntityFrameworkCore;
using SmartRecruitment.Domain.Entities;

namespace SmartRecruitment.Infrastructure.Persistence
{
    // Contains ONLY the tables this service owns. Schema/migrations are owned by src/Database.
    public class CandidateDbContext : DbContext
    {
        public CandidateDbContext(DbContextOptions<CandidateDbContext> options)
            : base(options)
        {
        }

        public DbSet<Candidate> Candidates => Set<Candidate>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CandidateDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
