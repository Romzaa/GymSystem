using GymSystem.DAL.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystem.DAL.Models
{
    public class Member : Person 
    {
        public string? Photo { get; set; } = default!;
        public required HealthRecord HealthRecord { get; set; }
        public ICollection<Membership> Memberships { get; set; } = default!;
        public ICollection<Booking> Bookings { get; set; } = default!;


    }
}
