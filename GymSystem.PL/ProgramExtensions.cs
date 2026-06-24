using GymSystem.DAL;
using GymSystem.DAL.GymDataSeed;
using Microsoft.EntityFrameworkCore;

namespace GymSystem.PL
{
    public static class ProgramExtensions
    {
        public static async Task MigrateAndSeedAsync(this WebApplication app)
        {

            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<GymDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            var seedPath = Path.Combine(app.Environment.ContentRootPath, "wwwroot", "Files");
            var pending = await dbContext.Database.GetPendingMigrationsAsync();
            if (pending.Any())
            {
                logger.LogInformation($"Applying {pending.Count()} Pending Migrations ");
                await dbContext.Database.MigrateAsync();
            }
            await GymDataSeed.SeedAsync(dbContext, logger, seedPath);


        }
    }
}
