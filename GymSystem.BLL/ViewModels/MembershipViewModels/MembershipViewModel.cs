using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystem.BLL.ViewModels.MembershipViewModels
{
    public class MembershipViewModel
    {
        public int PlandId { get; set; }
        public int MemberId { get; set; }
        public string PlanName { get; set; } = default!;
        public string MemberName { get; set; } = default!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
