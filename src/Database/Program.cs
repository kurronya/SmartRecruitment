using Microsoft.EntityFrameworkCore;
using SmartRecruitment.Infrastructure.Persistence;

// Usage: dotnet run --project src/Database -- migrate
if (args.Contains("migrate"))
{
    using var context = new SmartRecruitmentDbContext(DatabaseSettings.BuildOptions());
    await context.Database.MigrateAsync();
    Console.WriteLine("Migrations applied.");
}
else
{
    Console.WriteLine("This project owns the database schema. Pass 'migrate' to apply pending migrations.");
}
