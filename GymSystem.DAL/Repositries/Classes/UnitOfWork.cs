using GymSystem.DAL.Models;
using GymSystem.DAL.Repositries.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystem.DAL.Repositries.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GymDbContext _dbContext;
        private readonly Dictionary<string, object> repositories = [];

        public ISessionRepository SessionRepository { get; }

        public IMembershipRepository MembershipRepository { get; }

        public IBookingRepository BookingRepository { get; }

        public UnitOfWork(GymDbContext dbContext, ISessionRepository sessionRepo,
                           IMembershipRepository membershipRepository, IBookingRepository bookingRepository)
        {
            _dbContext = dbContext;
            SessionRepository = sessionRepo;
            MembershipRepository = membershipRepository;
            BookingRepository = bookingRepository;
        }


        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            var typeName = typeof(TEntity).Name;

            if (repositories.TryGetValue(typeName, out var Trepo)) 
            {
                return (IGenericRepository<TEntity>)Trepo;
            };
            var repo = new GenericRepository<TEntity>(_dbContext);
            repositories[typeName] = repo;
            
            return repo;
        }

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            return await _dbContext.SaveChangesAsync(ct);
        }
    }
}
