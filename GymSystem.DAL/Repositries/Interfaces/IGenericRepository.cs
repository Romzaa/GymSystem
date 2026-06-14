using GymSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace GymSystem.DAL.Repositries.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity 
    {
        Task<IEnumerable<T>> GetAllAsync(bool tracking = false, CancellationToken ct =default);
        Task<T?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<int> AddAsync(T Entity, CancellationToken ct = default);
        Task<int> UpdateAsync(T Entity, CancellationToken ct = default);
        Task<int> DeleteAsync(T Entity, CancellationToken ct = default);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate , CancellationToken ct = default);
    }
}
