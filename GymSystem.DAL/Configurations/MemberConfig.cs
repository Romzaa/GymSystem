using GymSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystem.DAL.Configurations
{
    internal class MemberConfig : IEntityTypeConfiguration<Member>
    {
       
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.Property(m => m.Name)
                       .HasMaxLength(50);

            builder.Property(m => m.Email)
                .HasMaxLength(100);
            builder.HasIndex(m => m.Email).IsUnique();
            builder.HasIndex(m => m.Phone).IsUnique();

            builder.Property(m => m.Gender)
                .HasConversion<string>().HasMaxLength(10);

            builder.ToTable( m =>
            {
                m.HasCheckConstraint("CK_Member_Email", "Email LIKE '%@%.%'");
                m.HasCheckConstraint("CK_Member_Phone", "Phone Like '01[0125][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'");
            });

            builder.OwnsOne(m => m.Address, a =>
            {
                a.Property(ad => ad.BuildingNumber).HasColumnName("BuildingNo");
                a.Property(ad => ad.Street).HasColumnType("varchar").HasMaxLength(30).HasColumnName("Street");
                a.Property(ad => ad.City).HasColumnType("varchar").HasMaxLength(30).HasColumnName("City");

            });
            builder.Property(m => m.JoinDate).HasColumnName("JoinDate").HasDefaultValueSql("GetDate()");
            builder.Property(m => m.Id).HasDefaultValueSql("NEXT VALUE FOR PersonSequence");

        }
    }
}
