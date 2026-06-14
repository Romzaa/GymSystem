using GymSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystem.DAL.Configurations
{
    internal class BookingConfig : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.HasKey(b => new { b.MemberId, b.SessionId });
            builder.Property(b => b.BookingDate).HasDefaultValueSql("GetDate()");

            builder.HasOne(b => b.Member)
                   .WithMany(m => m.Bookings)
                   .HasForeignKey(b => b.MemberId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(b => b.Session)
                   .WithMany(s => s.Bookings)
                   .HasForeignKey(b => b.SessionId)
                   .OnDelete(DeleteBehavior.NoAction);
        }

    }
}
