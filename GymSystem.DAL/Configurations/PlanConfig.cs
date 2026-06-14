using GymSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystem.DAL.Configurations
{
    internal class PlanConfig : IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> builder)
        {
            builder.Property(p => p.Name).HasColumnType("varchar").HasMaxLength(50);
            builder.Property(p => p.Description).HasColumnType("varchar").HasMaxLength(200);
            builder.Property(p => p.Price).HasPrecision(10, 2);
            builder.ToTable(p => p.HasCheckConstraint("CK_Plan_DurationDays", "DurationDays between 1 and 365"));


        }
    }
}
