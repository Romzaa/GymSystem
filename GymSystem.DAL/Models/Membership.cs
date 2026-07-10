using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GymSystem.DAL.Models
{
    public class Membership : BaseEntity
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [NotMapped]
        public bool IsActive =>
           StartDate <= DateTime.Now && EndDate >= DateTime.Now ? true : false;
        [NotMapped]
        public string Status =>
            IsActive ? "Active" : "Expired";

        public Member Member { get; set; } = default!;
        public int MemberId { get; set; }

        public Plan Plan { get; set; } = default!;
        public int PlanId { get; set; }
    }
}
