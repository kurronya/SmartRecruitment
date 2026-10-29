using Microsoft.EntityFrameworkCore;
using SmartRecruitment.Domain.Entities;

namespace SmartRecruitment.Infrastructure.Persistence
{
    // Contains ONLY the tables this service owns. Schema/migrations are owned by src/Database.
    public class RecruitmentDbContext : DbContext
    {
        public RecruitmentDbContext(DbContextOptions<RecruitmentDbContext> options)
            : base(options)
        {
        }

        public DbSet<JobDescription> JobDescriptions => Set<JobDescription>();
        public DbSet<Interview> Interviews => Set<Interview>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(RecruitmentDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
