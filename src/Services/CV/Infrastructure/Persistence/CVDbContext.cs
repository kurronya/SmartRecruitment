using Microsoft.EntityFrameworkCore;
using SmartRecruitment.Domain.Entities;

namespace SmartRecruitment.Infrastructure.Persistence
{
    // Contains ONLY the tables this service owns. Schema/migrations are owned by src/Database.
    public class CVDbContext : DbContext
    {
        public CVDbContext(DbContextOptions<CVDbContext> options)
            : base(options)
        {
        }

        public DbSet<CV> CVs => Set<CV>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CVDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
