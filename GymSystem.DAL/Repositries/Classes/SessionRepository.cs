using GymSystem.DAL.Models;
using GymSystem.DAL.Repositries.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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

        public async Task<IEnumerable<Session>> GetAllSessionswithTrainerandCategory(Expression<Func<Session,bool>>? filter,CancellationToken ct = default)
        {
            var sessions = await _dbContext.Sessions.Include(s => s.Category).Include(s => s.Trainer).ToListAsync(ct);

            return  sessions;
        }

        public async Task<int> GetAvailableSlotsAsync(int Id, CancellationToken ct =default)
        {
            var session =  _dbContext.Sessions.Include(s => s.Bookings).FirstOrDefault(s => s.Id == Id);
            if (session is null)
                return 0;
            return session.AvailableSlots;
        
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
