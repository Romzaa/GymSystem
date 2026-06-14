using GymSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystem.DAL.Repositries.Classes
{
    public class PlanRepository : GenericRepository<Plan>
    {
        public PlanRepository(GymDbContext dbContext) : base(dbContext)
        {
        }
    }
}
