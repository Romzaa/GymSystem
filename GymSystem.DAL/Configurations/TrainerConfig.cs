using GymSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystem.DAL.Configurations
{
    internal class TrainerConfig : IEntityTypeConfiguration<Trainer>
    {
        public void Configure(EntityTypeBuilder<Trainer> builder)
        {
            builder.Property(t => t.Name)
           .HasMaxLength(50);

            builder.Property(t => t.Email)
                .HasMaxLength(100);

            builder.HasIndex(m => m.Email).IsUnique();
            builder.HasIndex(m => m.Phone).IsUnique();

            builder.Property(m => m.Gender)
                .HasConversion<string>().HasMaxLength(10);

            builder.Property(m => m.Specialities)
                .HasConversion<string>().HasMaxLength(20);


            builder.ToTable( m =>
            {
                m.HasCheckConstraint("CK_Trainer_Email", "Email LIKE '%@%.%'");
                m.HasCheckConstraint("CK_Trainer_Phone", "Phone Like '01[0125][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'");
            });

            builder.OwnsOne(t => t.Address, a =>
            {
                a.Property(ad => ad.BuildingNumber).HasColumnName("BuildingNo");
                a.Property(ad => ad.Street).HasColumnType("varchar").HasMaxLength(30).HasColumnName("Street");
                a.Property(ad => ad.City).HasColumnType("varchar").HasMaxLength(30).HasColumnName("City");

            });
            builder.Property(t => t.JoinDate).HasColumnName("HireDate").HasDefaultValueSql("GetDate()");
            builder.Property(t => t.Id).HasDefaultValueSql("NEXT VALUE FOR PersonSequence");
        }
    }
}
