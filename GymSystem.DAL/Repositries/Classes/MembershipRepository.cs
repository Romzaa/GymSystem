using GymSystem.DAL.Models;
using GymSystem.DAL.Repositries.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace GymSystem.DAL.Repositries.Classes
{
    public class MembershipRepository : GenericRepository<Membership>, IMembershipRepository
    {
        private readonly GymDbContext _dbContext;

        public MembershipRepository(GymDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Membership>> GetAllMembershipsWithPlansAndMembers(Expression<Func<Membership, bool>>? filter = null, CancellationToken ct = default)
        {
            IQueryable<Membership> memberships =  _dbContext.Memberships.AsNoTracking().Include(ms => ms.Plan).Include(ms => ms.Member);
            if(filter is not null)
            {
                memberships = memberships.Where(filter);
            }
            return await memberships.ToListAsync(ct);
        }

        public async Task<Membership?> GetMembershipWithPlanAndMember(int Id, CancellationToken ct = default)
        {
            var membership= await _dbContext.Memberships.Include(ms => ms.Plan).Include(ms => ms.Member).FirstOrDefaultAsync(ms => ms.Id == Id, ct);
            if (membership is null) return null;
            return membership;
        }

    }
}
