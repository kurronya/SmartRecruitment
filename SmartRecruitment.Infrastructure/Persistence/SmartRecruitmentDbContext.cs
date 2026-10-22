using Microsoft.EntityFrameworkCore;
using SmartRecruitment.Domain.Entities;

namespace SmartRecruitment.Infrastructure.Persistence
{
    public class SmartRecruitmentDbContext : DbContext
    {
        public SmartRecruitmentDbContext(DbContextOptions<SmartRecruitmentDbContext> options)
            : base(options)
        {
        }

        public DbSet<Candidate> Candidates => Set<Candidate>();
        public DbSet<Company> Companies => Set<Company>();
        public DbSet<JobDescription> JobDescriptions => Set<JobDescription>();
        public DbSet<Interview> Interviews => Set<Interview>();
        public DbSet<CV> CVs => Set<CV>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Automatically applies every IEntityTypeConfiguration<T> class found in this assembly
            // (the Configurations/ files we'll write next), instead of registering each one manually
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SmartRecruitmentDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}