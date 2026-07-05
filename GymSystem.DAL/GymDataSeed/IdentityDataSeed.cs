using GymSystem.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystem.DAL.GymDataSeed
{
    public static class IdentityDataSeed
    {
        public static async Task SeedAsync(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager,
                                        ILogger logger, CancellationToken ct = default)
        {

            try 
            {
                bool hasRoles = roleManager.Roles.Any();
                bool hasUsers = userManager.Users.Any();
                if (hasRoles && hasUsers) return;

                if (!hasRoles)
                {
                    var roles = new List<IdentityRole>
                    {
                        new IdentityRole { Name = "SuperAdmin" },
                        new IdentityRole { Name = "Admin" }
                    };

                    foreach(var role in roles.Select(r => r.Name))
                    {
                        if(! await roleManager.RoleExistsAsync(role!))
                        {
                         var result = await roleManager.CreateAsync(new IdentityRole (role!));
                            if(!result.Succeeded)
                            {
                                logger.LogError($"Failed To Create Role {role}, {string.Join(", ", result.Errors.Select(e => e.Description))} ");
                            }

                        }
                    }
                }
                if (!hasUsers)
                {
                    var superdAdmin = new ApplicationUser { 
                            FirstName= "Ahmad",
                            LastName = "Mohamad",
                            UserName = "AhmadMohamad",
                            Email = "ahmad@mail.com",
                            PhoneNumber="01234567890"
                            
                                                    };

                    var superAdminResult = await userManager.CreateAsync(superdAdmin, "P@ssw0rd");
                    if (superAdminResult.Succeeded)
                    {
                        await userManager.AddToRoleAsync(superdAdmin, "SuperAdmin");
                    }
                    else
                    {
                        logger.LogError($"Failed To Create Super Admin User , {string.Join(", ", superAdminResult.Errors.Select(e=> e.Description))}");
                        return;
                    }

                    var admin = new ApplicationUser
                    {
                        FirstName = "Omar",
                        LastName = "Ahmad",
                        UserName = "OmarAhmad",
                        Email = "omar@mail.com",
                        PhoneNumber = "01236547890"

                    };
                    var adminResult = await userManager.CreateAsync(admin, "P@ssw0rd");
                    if (adminResult.Succeeded)
                    {
                        await userManager.AddToRoleAsync(admin, "Admin");
                    }
                    else
                    {
                        logger.LogError($"Failed To Create Admin User ,{string.Join(", ", adminResult.Errors.Select(e=> e.Description))}");
                        return;
                    }
                    logger.LogInformation($"Data Seeded Successfully Superd Admin AS {superdAdmin.Email} , Admin As {admin.Email}");
                }
            }
            catch (Exception ex) 
            {
                logger.LogError(ex, "Identity Seeding Failed");
                throw;                
            };

        }
    }
}
