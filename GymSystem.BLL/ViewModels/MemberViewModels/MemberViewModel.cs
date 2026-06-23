using GymSystem.DAL.Enums;
using GymSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystem.BLL.ViewModels.MemberViewModels
{
    public class MemberViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Phone { get; set; } = default!;
        public string? Photo { get; set; } = default!;
        public Gender Gender { get; set; }
        public string DateOfBirth { get; set; } = default!;
        public string Address { get; set; } = default!;
        public string PlanName { get; set; } = default!;
        public string MembershipStartDate { get; set; } = default!;
        public string MembershipEndDate { get; set; } = default!;
    } 
}
