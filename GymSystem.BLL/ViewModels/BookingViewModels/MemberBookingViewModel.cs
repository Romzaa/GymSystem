using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystem.BLL.ViewModels.BookingViewModels
{
    public class MemberBookingViewModel
    {
            public int MemberId { get; set; }
            public int SessionId { get; set; }
            public string MemberName { get; set; } = default!;
            public string BookingDate { get; set; } = default!;
            public bool IsAttended { get; set; } = false;
            public int AvailableSlots { get; set; }


    }
}
