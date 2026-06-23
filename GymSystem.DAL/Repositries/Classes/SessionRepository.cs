using GymSystem.DAL.Models;
using GymSystem.DAL.Repositries.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystem.DAL.Repositries.Classes
{
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        private readonly GymDbContext _dbContext;

        public SessionRepository(GymDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Session>> GetAllSessionswithTrainerandCategory(CancellationToken ct = default)
        {
            var sessions =  _dbContext.Sessions.Include(s => s.Category).Include(s => s.Trainer);

            return await sessions.ToListAsync(ct);
        }
        public async Task<Session?> GetSessionwithTrainerandCategoryByIdAsync(int Id, CancellationToken ct = default)
        {
            var session = await  _dbContext.Sessions.Include(s => s.Category).Include(s => s.Trainer).FirstOrDefaultAsync(s=> s.Id == Id);
            if(session is null)
                return null;
            return  session;
        }
    }
}
