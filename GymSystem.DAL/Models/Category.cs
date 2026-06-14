using GymSystem.DAL.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystem.DAL.Models
{
    public class Category : BaseEntity
    {
        public int Id { get; set; }
        public TrainingType Name { get; set; }

        public ICollection<Session> Sessions { get; set; } = default!;

    }
}
