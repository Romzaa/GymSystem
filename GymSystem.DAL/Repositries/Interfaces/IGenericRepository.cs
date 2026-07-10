using GymSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace GymSystem.DAL.Repositries.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity 
    {
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T,bool>>? filter, bool tracking = false, CancellationToken ct =default);
        Task<T?> GetByIdAsync(int id, CancellationToken ct = default);
        void AddAsync(T Entity);
        void UpdateAsync(T Entity);
        void DeleteAsync(T Entity);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate , CancellationToken ct = default);
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, bool tracking = false, CancellationToken ct = default);
    }
}
