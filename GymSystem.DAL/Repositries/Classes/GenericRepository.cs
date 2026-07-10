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
            public  async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T,bool>>? filter,bool tracking = false, CancellationToken ct = default)
            {
                IQueryable<T> query = tracking ? _dbSet : _dbSet.AsNoTracking();
                if(filter is not null)
            {
                query = query.Where(filter);

            }
            return await query.ToListAsync(ct);

        }

        public async Task<T?> GetByIdAsync(int id, CancellationToken ct = default)
        {
                        var Entity = _dbSet.FindAsync(id, ct);
            return await Entity;
        }
        public void AddAsync(T Entity)
        {
            _dbSet.Add(Entity);
        }

        public void UpdateAsync(T Entity)
        {
            _dbSet.Update(Entity);
        }
        public void DeleteAsync(T Entity)
        {
            _dbSet.Remove(Entity);
        }


       public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken ct)
        {
            return await _dbSet.AnyAsync(predicate, ct);

        }

        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, bool tracking = false, CancellationToken ct = default)
        {
            var result = tracking ? _dbSet.FirstOrDefaultAsync(predicate, ct) : _dbSet.AsNoTracking().FirstOrDefaultAsync(predicate, ct);
           
            return await result;
        }
    }
}
