using GymSystem.DAL.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystem.DAL.Models
{
    public class Trainer : Person
    {
        public TrainingType Specialities { get; set; }
        public ICollection<Session> Sessions { get; set; } = default!;
    }
}
