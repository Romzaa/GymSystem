using GymSystem.DAL.Enums;
using GymSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace GymSystem.DAL
{
    public class GymDbContext : DbContext
    {

        public GymDbContext( DbContextOptions<GymDbContext> options ) : base( options ) 
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasSequence<int>("PersonSequence")
                .StartsAt(1)
                .IncrementsBy(1);
            modelBuilder.Ignore<Person>();
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());


            #region SEEDING...

            // SEEDING CATEGORIES .......
            modelBuilder.Entity<Category> ().HasData(
                new Category { Id = 1, Name = TrainingType.Strength},
                new Category { Id = 2, Name = TrainingType.Cardio},
                new Category { Id = 3, Name = TrainingType.Yoga},
                new Category { Id = 4, Name = TrainingType.Boxing},
                new Category { Id = 5, Name = TrainingType.CrossFit}
            );

            #endregion

        }

        public DbSet<Member> Members { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<Session> Sessions { get; set; }







    }
}
