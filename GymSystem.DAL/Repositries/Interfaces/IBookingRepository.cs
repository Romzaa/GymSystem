using GymSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace GymSystem.DAL.Repositries.Interfaces
{
    public interface IBookingRepository : IGenericRepository<Booking>
    {
        Task<IEnumerable<Booking>> GetAllBookingsWithMembersAndSessions(Expression<Func<Booking, bool>> filter ,CancellationToken ct = default);
        Task<Booking?> GetBookingWithMemberAndSession(int Id, CancellationToken ct = default);
    }
}
