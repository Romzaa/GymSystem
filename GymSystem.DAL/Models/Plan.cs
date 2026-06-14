using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystem.DAL.Models
{
    public class Plan : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }    = default!;
        public string Description { get; set; } = default!;
        public decimal Price { get; set; }
        public int DurationDays { get; set; }
        public bool IsActive { get; set; } = false;

        public ICollection<Membership> Memberships { get; set; } = default!;


    }
}
