using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GymSystem.DAL.Models
{
    public class Membership : BaseEntity
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string Status => EndDate > StartDate ? "Active" : "Expired";
        public bool IsActive => EndDate > StartDate;

        public Member Member { get; set; } = default!;
        public int MemberId { get; set; }

        public Plan Plan { get; set; } = default!;
        public int PlanId { get; set; }
    }
}
