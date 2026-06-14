using GymSystem.DAL.Models;
using GymSystem.DAL.Repositries.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace GymSystem.DAL.Repositries.Classes
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity, new()
    {
        private readonly GymDbContext _dbcontext;
        private readonly DbSet<T> _dbSet;
        public GenericRepository(GymDbContext dbContext) 
        {
            _dbcontext = dbContext;
            _dbSet = _dbcontext.Set<T>();
        }

        // Cann't Use ( Include ) Method Here Because We Don't Know The Type Of T..
        // Only The Specific Repository Can Use It
        public async Task<IEnumerable<T>> GetAllAsync(bool tracking = false, CancellationToken ct = default)
        {
            var Entities = tracking ? _dbSet.ToListAsync(ct) : _dbSet.AsNoTracking().ToListAsync(ct);
            return await Entities;
        }

        public async Task<T?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var Entity = _dbSet.FindAsync(id, ct);
            return await Entity;
        }
        public async Task<int> AddAsync(T Entity, CancellationToken ct = default)
        {
            _dbSet.Add(Entity);
            return await _dbcontext.SaveChangesAsync(ct);
        }

        public async Task<int> UpdateAsync(T Entity, CancellationToken ct = default)
        {
            _dbSet.Update(Entity);
            return await _dbcontext.SaveChangesAsync(ct);
        }
        public async Task<int> DeleteAsync(T Entity, CancellationToken ct = default)
        {
            _dbSet.Remove(Entity);
            return await _dbcontext.SaveChangesAsync(ct);
        }


       public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken ct)
        {
            return await _dbSet.AnyAsync(predicate, ct);

        }
    }
}
