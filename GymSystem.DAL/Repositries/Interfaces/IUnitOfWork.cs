using GymSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystem.DAL.Repositries.Interfaces
{
    public interface IUnitOfWork
    {
        IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new();
        Task<int> SaveChangesAsync(CancellationToken ct = default);

        ISessionRepository SessionRepository { get; }
        IMembershipRepository MembershipRepository { get; }
        IMembershipRepository BookingRepository { get; }

    }
}
