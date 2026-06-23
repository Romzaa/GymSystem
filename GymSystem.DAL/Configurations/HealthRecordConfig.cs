using GymSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystem.DAL.Configurations
{
    internal class HealthRecordConfig : IEntityTypeConfiguration<HealthRecord> 
    {
        public void Configure(EntityTypeBuilder<HealthRecord> builder)
        {
            builder.HasOne(hr => hr.Member)
                   .WithOne(m => m.HealthRecord)
                   .HasForeignKey<HealthRecord>(hr => hr.MemberId);
            builder.Property(hr => hr.BloodType).HasConversion<string>();
            builder.Property(hr => hr.Height).HasPrecision(5,2);
            builder.Property(hr => hr.Weight).HasPrecision(5, 2);
            builder.Property(hr => hr.Note).HasMaxLength(500);
        }
    }
}
