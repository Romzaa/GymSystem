using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystem.DAL.Models
{
    public class Session : BaseEntity
    {
        public int Id { get; set; }
        public int Capacity { get; set; }
        public string Description { get; set; } = default!;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public Category Category { get; set; } = default!;
        public int CategoryId { get; set; }
        public Trainer Trainer { get; set; } = default!;
        public int TrainerId { get; set; }
        public ICollection<Booking> Bookings { get; set; } = [];

        public int AvailableSlots => Capacity - (Bookings?.Count?? 0) ;
        
    }
}
