using GymSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystem.DAL.Repositries.Interfaces
{
    public interface ISessionRepository : IGenericRepository<Session>
    {
        public Task<IEnumerable<Session>> GetAllSessionswithTrainerandCategory(CancellationToken ct =default);
        public Task<Session?> GetSessionwithTrainerandCategoryByIdAsync(int Id, CancellationToken ct = default);

    }
}
