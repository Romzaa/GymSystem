using GymSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GymSystem.DAL.GymDataSeed
{
    public static class GymDataSeed
    {
        public static async Task SeedAsync(GymDbContext dbContext, 
                                     ILogger Logger ,
                                     string SeedFilepath,
                                     CancellationToken ct = default
                                        )
        {
            try 
            {
                if(!await dbContext.Plans.AnyAsync(ct))
                {
                    var plans = LoadDataFromJsonFile<Plan>("plans.json", SeedFilepath);
                    if(plans.Count > 0)
                    {
                        dbContext.Plans.AddRange(plans);
                        Logger.LogInformation($"Seeded {plans.Count} plans");

                        if ( dbContext.ChangeTracker.HasChanges()) await dbContext.SaveChangesAsync(ct); 
                    }
                }
            
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed To Data Seed");
                throw ;
            }
    }


        private static List<T> LoadDataFromJsonFile<T>(string fileName, string folderPath) 
        {
            var filePath = Path.Combine(folderPath, fileName);
            if (!File.Exists(filePath)) throw new FileNotFoundException($"Seed Data File Is Not Found : {filePath}");
            var data = File.ReadAllText(filePath);
            var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            options.Converters.Add(new JsonStringEnumConverter());
            return JsonSerializer.Deserialize<List<T>>(data) ?? [];
        }

}
}
