using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystem.BLL.ViewModels.MembershipViewModels
{
    public class CreateMembershipViewModel
    {
        public int PlanId { get; set; }
        public int MemberId { get; set; }
        public DateTime? StartDate { get; set; }
    }
}
