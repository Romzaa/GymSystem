using GymSystem.DAL.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystem.DAL.Models
{
    public class HealthRecord : BaseEntity
    {
        public int Id { get; set; }
        public string? Note { get; set; }
        public DateTime LastUpdate { get; set; }
        public BloodType BloodType { get; set; }
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        public  Member Member { get; set; } = default!;
        public int MemberId { get; set; }

    }
}
