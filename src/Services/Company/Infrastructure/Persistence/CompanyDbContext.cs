using Microsoft.EntityFrameworkCore;
using SmartRecruitment.Domain.Entities;

namespace SmartRecruitment.Infrastructure.Persistence
{
    // Contains ONLY the tables this service owns. Schema/migrations are owned by src/Database.
    public class CompanyDbContext : DbContext
    {
        public CompanyDbContext(DbContextOptions<CompanyDbContext> options)
            : base(options)
        {
        }

        public DbSet<Company> Companies => Set<Company>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CompanyDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
