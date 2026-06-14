using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystem.DAL.Models
{
    public abstract class BaseEntity
    {
        public DateTime UpdatedAt { get; set; }
    }
}
