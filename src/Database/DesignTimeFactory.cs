using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SmartRecruitment.Infrastructure.Persistence
{
    public static class DatabaseSettings
    {
        // Reads ConnectionStrings:DefaultConnection from user-secrets (local) or environment variables (Render/CI)
        public static string GetConnectionString()
        {
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets(typeof(DatabaseSettings).Assembly, optional: true)
                .AddEnvironmentVariables()
                .Build();

            return configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException(
                    "ConnectionStrings:DefaultConnection is not configured (use dotnet user-secrets or an environment variable).");
        }

        public static DbContextOptions<SmartRecruitmentDbContext> BuildOptions()
        {
            return new DbContextOptionsBuilder<SmartRecruitmentDbContext>()
                .UseNpgsql(GetConnectionString())
                .Options;
        }
    }

    // Lets `dotnet ef` create the DbContext without starting a web host
    public class SmartRecruitmentDbContextFactory : IDesignTimeDbContextFactory<SmartRecruitmentDbContext>
    {
        public SmartRecruitmentDbContext CreateDbContext(string[] args)
        {
            return new SmartRecruitmentDbContext(DatabaseSettings.BuildOptions());
        }
    }
}
