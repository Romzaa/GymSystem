using GymSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace GymSystem.DAL.Repositries.Interfaces
{
    public interface IMembershipRepository : IGenericRepository<Membership>
    {
       Task<IEnumerable<Membership>> GetAllMembershipsWithPlansAndMembers(Expression<Func<Membership, bool>>? filter = null, CancellationToken ct =default);
       Task<Membership?> GetMembershipWithPlanAndMember(int Id , CancellationToken ct = default);
    }
}
