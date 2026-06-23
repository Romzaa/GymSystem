using GymSystem.DAL.Enums;
using GymSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics.X86;
using System.Text;

namespace GymSystem.BLL.ViewModels.SessionViewModels
{
    public class SessionViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; } = default!;
        public int Capacity { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string TrainerName { get; set; } = default!;
        public TrainingType CategoryName { get; set; } 
        public int AvailableSlots { get; set; }

        // Computed properties
        public string DateDisplay => $"{StartTime:MMM dd , yyyy}";
        public string TimeRangeDisplay => $"{StartTime:hh:mm tt} - {EndTime:hh:mm tt}";
        public TimeSpan Duration => EndTime - StartTime;
        public string Status
        {
            get
            {
                if (StartTime > DateTime.Now)
                    return "Upcoming";
                else if (StartTime <= DateTime.Now && EndTime >= DateTime.Now)
                    return "Ongoing";
                else
                    return "Completed";
            }
        }
    }
}
