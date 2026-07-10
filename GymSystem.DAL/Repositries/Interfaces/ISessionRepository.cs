using GymSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace GymSystem.DAL.Repositries.Interfaces
{
    public interface ISessionRepository : IGenericRepository<Session>
    {
        public Task<IEnumerable<Session>> GetAllSessionswithTrainerandCategory(Expression<Func<Session,bool>>? filter,CancellationToken ct =default);
        public Task<Session?> GetSessionwithTrainerandCategoryByIdAsync(int Id, CancellationToken ct = default);
        public Task<int> GetAvailableSlotsAsync(int Id, CancellationToken ct =default);

    }
}
